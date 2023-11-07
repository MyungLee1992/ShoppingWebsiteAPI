using AutoMapper;
using Moq;
using ShoppingWebsiteAPI.Models;
using ShoppingWebsiteAPI.Repositories;
using ShoppingWebsiteAPI.Services;

namespace ServiceLayer.Tests.NUnit
{
    [TestFixture]
    public class ItemServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IMapper> _mapper;
        private ItemService _itemService;
        private Item item;
        private ItemDto itemDto;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _mapper = new Mock<IMapper>();
            _itemService = new ItemService(_unitOfWork.Object, _mapper.Object);

            Guid id = Guid.NewGuid();

            item = new Item
            {
                Id = id,
                Name = "Jacket",
                Description = "Men's Jacket",
                Type = "Cloth",
                Price = 32.48d,
                ImageUrl = "https://images.unsplash.com/photo-1583743814966-8936f5b7be1a?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=387&q=80"
            };

            itemDto = new ItemDto
            {
                Id = id,
                Name = "Jacket",
                Description = "Men's Jacket",
                Type = "Cloth",
                Price = 32.48d,
                ImageUrl = "https://images.unsplash.com/photo-1583743814966-8936f5b7be1a?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=387&q=80"
            };
        }

        [Test]
        public async Task GetItems_WithSampleItems_ReturnsAllItems()
        {
            // Arrange
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var items = new List<Item>
            {
                new Item 
                { 
                    Id = id1, 
                    Name = "Jacket", 
                    Description = "Men's Jacket", 
                    Type = "Cloth", 
                    Price = 32.48d,
                    ImageUrl = "https://images.unsplash.com/photo-1583743814966-8936f5b7be1a?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=387&q=80"
                },
                new Item
                {
                    Id = id2,
                    Name = "Pants",
                    Description = "Men's Pants",
                    Type = "Cloth",
                    Price = 50.35d,
                    ImageUrl = "https://images.unsplash.com/photo-1583743814966-8936f5b7be1a?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=387&q=80"
                },
                new Item
                {
                    Id = id3,
                    Name = "Shirts",
                    Description = "Men's Shirts",
                    Type = "Cloth",
                    Price = 60d,
                    ImageUrl = "https://images.unsplash.com/photo-1583743814966-8936f5b7be1a?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=387&q=80"
                },
            };

            var itemDtos = new List<ItemDto>
            {
                new ItemDto
                {
                    Id = id1,
                    Name = "Jacket",
                    Description = "Men's Jacket",
                    Type = "Cloth",
                    Price = 32.48d,
                    ImageUrl = "https://images.unsplash.com/photo-1583743814966-8936f5b7be1a?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=387&q=80"
                },
                new ItemDto
                {
                    Id = id2,
                    Name = "Pants",
                    Description = "Men's Pants",
                    Type = "Cloth",
                    Price = 50.35d,
                    ImageUrl = "https://images.unsplash.com/photo-1583743814966-8936f5b7be1a?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=387&q=80"
                },
                new ItemDto
                {
                    Id = id3,
                    Name = "Shirts",
                    Description = "Men's Shirts",
                    Type = "Cloth",
                    Price = 60d,
                    ImageUrl = "https://images.unsplash.com/photo-1583743814966-8936f5b7be1a?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=387&q=80"
                },
            };

            _unitOfWork.Setup(repo => repo.Items.GetAllItemsAsync()).ReturnsAsync(items);
            _mapper.Setup(m => m.Map<IEnumerable<ItemDto>>(It.IsAny<Item>())).Returns(itemDtos);

            // Act
            var result = await _itemService.GetItemsAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(itemDtos.Count));
        }

        [Test]
        public async Task GetItem_WithId_ReturnItem()
        {
            // Arrange
            _unitOfWork.Setup(repo => repo.Items.GetItemByIdAsync(item.Id)).ReturnsAsync(item);
            _mapper.Setup(m => m.Map<ItemDto>(It.IsAny<Item>())).Returns(itemDto);

            // Act
            var result = await _itemService.GetItemAsync(item.Id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(itemDto));
        }

        [Test]
        public async Task CreateItem_WithItemDtoNull_ReturnFalse()
        {
            // Act
            var result = await _itemService.CreateItemAsync(null!);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task CreateItem_WithItemDto_ReturnTrue()
        {
            // Arrange
            _mapper.Setup(m => m.Map<Item>(It.IsAny<ItemDto>())).Returns(item);
            _unitOfWork.Setup(repo => repo.Items.Add(item));

            // Act
            var result = await _itemService.CreateItemAsync(itemDto);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task UpdateItem_WithNoItemFound_ReturnFalse()
        {
            // Arrange
            var id = new Guid();
            _unitOfWork.Setup(repo => repo.Items.GetItemByIdAsync(id)).ReturnsAsync(() => null);

            // Act
            var result = await _itemService.UpdateItemAsync(itemDto);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task UpdateItem_WithoutItemDto_ReturnFalse()
        {
            // Arrange
            _unitOfWork.Setup(repo => repo.Items.GetItemByIdAsync(itemDto.Id)).ReturnsAsync(() => null);

            // Act
            var result = await _itemService.UpdateItemAsync(null!);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task UpdateItem_WithoutIdAndItemDto_ReturnTrue()
        {
            // Arrange
            _unitOfWork.Setup(repo => repo.Items.GetItemByIdAsync(item.Id)).ReturnsAsync(item);
            _unitOfWork.Setup(repo => repo.Items.UpdateItem(item));

            // Act
            var result = await _itemService.CreateItemAsync(itemDto);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task DeleteItem_WithNoItemFound_ReturnFalse()
        {
            // Arrange
            var id = new Guid();
            _unitOfWork.Setup(repo => repo.Items.GetItemByIdAsync(id)).ReturnsAsync(() => null);

            // Act
            var result = await _itemService.DeleteItemAsync(id);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeleteItem_WithId_ReturnTrue()
        {
            // Arrange
            _unitOfWork.Setup(repo => repo.Items.GetItemByIdAsync(item.Id)).ReturnsAsync(item);
            _unitOfWork.Setup(repo => repo.Items.RemoveItem(item));

            // Act
            var result = await _itemService.DeleteItemAsync(item.Id);

            // Assert
            Assert.That(result, Is.True);
        }

    }
}