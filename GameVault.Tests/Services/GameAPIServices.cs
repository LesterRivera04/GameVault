using GameVault.API.DTO;
using GameVault.API.Models;
using GameVault.API.Repository;
using GameVault.API.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace GameVault.Tests.Services
{
    public class GameAPIServicesTests
    {

        /*Test #1*/
        /*Este test valida que se convirte la entidad Game a GameDto*/
        [Fact]
        public async Task GetAllGamesAsync_ReturnsMappedGameDto()
        {
            var mockRepo = new Mock<IGameRepository>();

            mockRepo.Setup(r => r.GetAllGamesAsync())
                .ReturnsAsync(new List<Game>
                {
                    new Game
                    {
                        Id = 1,
                        Name = "Zelda",
                        ReleaseDate = DateTime.Now,
                        Rating = 10,
                        ImageUrl = "img",
                        GameGenres = new List<GameGenre>
                        {
                            new GameGenre
                            {
                                Genre = new Genre { Name = "Adventure" }
                            }
                        },
                        GamePlatforms = new List<GamePlatform>
                        {
                            new GamePlatform
                            {
                                Platform = new Platform { Name = "Switch" }
                            }
                        }
                    }
                });

            var service = new GameAPIServices(
                mockRepo.Object,
                Mock.Of<IGenreRepository>(),
                Mock.Of<IPlatformRepository>(),
                new HttpClient(),
                Mock.Of<IConfiguration>()
            );

            var result = await service.GetAllGamesAsync();

            var game = result.First();

            Assert.Equal("Zelda", game.Name);
            Assert.Single(game.Genres);
            Assert.Single(game.Platforms);
            Assert.Equal("Adventure", game.Genres.First());
            Assert.Equal("Switch", game.Platforms.First());
        }


        /*Test #2*/
        /*Valida que se crea un juego correctamente*/
        [Fact]
        public async Task AddGameAsync_CreatesGameWithRelations()
        {
            var mockRepo = new Mock<IGameRepository>();

            Game capturedGame = null;

            mockRepo.Setup(r => r.AddGameAsync(It.IsAny<Game>()))
                    .Callback<Game>(g => capturedGame = g)
                    .Returns(Task.CompletedTask);

            var service = new GameAPIServices(
                mockRepo.Object,
                Mock.Of<IGenreRepository>(),
                Mock.Of<IPlatformRepository>(),
                new HttpClient(),
                Mock.Of<IConfiguration>()
            );

            var dto = new CreateGameDto
            {
                Name = "Halo",
                ReleaseDate = DateTime.Now,
                Rating = 9,
                ImageUrl = "img",
                GenreIds = new List<int> { 1, 2 },
                PlatformIds = new List<int> { 3 }
            };

            await service.AddGameAsync(dto);

            Assert.NotNull(capturedGame);
            Assert.Equal("Halo", capturedGame.Name);
            Assert.Equal(2, capturedGame.GameGenres.Count);
            Assert.Single(capturedGame.GamePlatforms);
        }


        /*Test #3*/
        /*Este valida que se actualizan los datos del juego de manera*/
        [Fact]
        public async Task UpdateGameAsync_UpdatesGameCorrectly()
        {
            var mockRepo = new Mock<IGameRepository>();

            var existingGame = new Game
            {
                Id = 1,
                Name = "Old Name",
                GameGenres = new List<GameGenre>(),
                GamePlatforms = new List<GamePlatform>()
            };

            mockRepo.Setup(r => r.GetGameByIdAsync(1))
                    .ReturnsAsync(existingGame);

            var service = new GameAPIServices(
                mockRepo.Object,
                Mock.Of<IGenreRepository>(),
                Mock.Of<IPlatformRepository>(),
                new HttpClient(),
                Mock.Of<IConfiguration>()
            );

            var dto = new EditGameDto
            {
                Name = "New Name",
                ReleaseDate = DateTime.Now,
                Rating = 8,
                ImageUrl = "img",
                GenreIds = new List<int> { 1 },
                PlatformIds = new List<int> { 2 }
            };

            await service.UpdateGameAsync(1, dto);

            Assert.Equal("New Name", existingGame.Name);
            Assert.Single(existingGame.GameGenres);
            Assert.Single(existingGame.GamePlatforms);

            mockRepo.Verify(r => r.UpdateGameAsync(existingGame), Times.Once);
        }


        /*Test #4*/
        /*Evita errores por si el juego no existe*/
        [Fact]
        public async Task UpdateGameAsync_DoesNothing_WhenGameNotFound()
        {
            var mockRepo = new Mock<IGameRepository>();

            mockRepo.Setup(r => r.GetGameByIdAsync(1))
                    .ReturnsAsync((Game)null);

            var service = new GameAPIServices(
                mockRepo.Object,
                Mock.Of<IGenreRepository>(),
                Mock.Of<IPlatformRepository>(),
                new HttpClient(),
                Mock.Of<IConfiguration>()
            );

            var dto = new EditGameDto
            {
                Name = "Test"
            };

            await service.UpdateGameAsync(1, dto);

            mockRepo.Verify(r => r.UpdateGameAsync(It.IsAny<Game>()), Times.Never);
        }


        /*Test #5*/
        /*El servicio llama al repositorio para asi eliminar el juego*/
        [Fact]
        public async Task DeleteGameAsync_CallsRepository()
        {
            var mockRepo = new Mock<IGameRepository>();

            var service = new GameAPIServices(
                mockRepo.Object,
                Mock.Of<IGenreRepository>(),
                Mock.Of<IPlatformRepository>(),
                new HttpClient(),
                Mock.Of<IConfiguration>()
            );

            await service.DeleteGameAsync(1);

            mockRepo.Verify(r => r.DeleteGameAsync(1), Times.Once);
        }


        /*Test #6*/
        /*El service devuelve el juego correctamente desde el repository*/
        [Fact]
        public async Task GetGameByIdAsync_ReturnsGameFromRepository()
        {
            var mockRepo = new Mock<IGameRepository>();

            var game = new Game { Id = 1, Name = "Halo" };

            mockRepo.Setup(r => r.GetGameByIdAsync(1))
                    .ReturnsAsync(game);

            var service = new GameAPIServices(
                mockRepo.Object,
                Mock.Of<IGenreRepository>(),
                Mock.Of<IPlatformRepository>(),
                new HttpClient(),
                Mock.Of<IConfiguration>()
            );

            var result = await service.GetGameByIdAsync(1);

            Assert.Equal(game, result);
        }
    }
}