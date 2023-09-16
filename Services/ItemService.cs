using AutoMapper;
using ShoppingWebsiteAPI.Models;
using ShoppingWebsiteAPI.Repositories;

namespace ShoppingWebsiteAPI.Services
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ItemService(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ItemDto>> GetItemsAsync()
        {
            var items = await _unitOfWork.Items.GetAllItemsAsync();

            return items.Select(item => _mapper.Map<ItemDto>(item));
        }

        public async Task<ItemDto?> GetItemAsync(Guid id)
        {
            var item = await _unitOfWork.Items.GetItemByIdAsync(id);

            return _mapper.Map<ItemDto>(item);
        }

        public async Task<bool> CreateItemAsync(ItemDto itemDto)
        {
            if (itemDto == null)
            {
                return false;
            }

            var item = _mapper.Map<Item>(itemDto);
            _unitOfWork.Items.Add(item);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<bool> UpdateItemAsync(Guid id, ItemDto itemDto)
        {
            var item = await _unitOfWork.Items.GetItemByIdAsync(id);
            if (item == null)
            {
                return false;
            }

            item = item with
            {
                Name = itemDto.Name,
                Description = itemDto.Description,
                Type = itemDto.Type,
                Price = itemDto.Price,
                ImageUrl = itemDto.ImageUrl,
            };

            _unitOfWork.Items.UpdateItem(item);

            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteItemAsync(Guid id)
        {
            var item = await _unitOfWork.Items.GetItemByIdAsync(id);
            if (item == null)
            {
                return false;
            }

            _unitOfWork.Items.RemoveItem(item);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
