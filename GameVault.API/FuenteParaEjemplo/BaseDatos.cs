using Microsoft.AspNetCore.Http.HttpResults;
using System.Data;
using System.Security.Principal;
using System.Xml.Linq;

namespace GameVault.API.FuenteParaEjemplo
{
    public class BaseDatos
    {
        /*
		CREATE DATABASE GameVault;
		GO

		USE GameVault;
		GO

		CREATE TABLE Usuarios (
			Id INT PRIMARY KEY IDENTITY (1,1),
			UserName NVARCHAR(50) NOT NULL,
			Email NVARCHAR(100) NOT NULL
		);
		GO

		CREATE TABLE Platforms (
			Id INT PRIMARY KEY IDENTITY (1,1),
			Name NVARCHAR(100) NOT NULL
		);
		GO

		CREATE TABLE Genres (
			Id INT PRIMARY KEY IDENTITY (1,1),
			Name NVARCHAR(100) NOT NULL
		);
		GO

		CREATE TABLE Games (
			Id INT PRIMARY KEY IDENTITY (1,1),
			Name NVARCHAR(100) NOT NULL,
			ReleaseDate DATETIME,
			Rating FLOAT DEFAULT 0,
			ImageUrl NVARCHAR(500),
			CreatedAt DATETIME DEFAULT GETDATE(),
			TimesAdded INT DEFAULT 0
		);
		GO

		CREATE TABLE UserGames (
			Id INT PRIMARY KEY IDENTITY (1,1),
			UserId INT NOT NULL,
			GameId INT NOT NULL,
			AddedAt DATETIME DEFAULT GETDATE(),
			IsFavorite BIT DEFAULT 0,
			CONSTRAINT FK_UserGames_Users FOREIGN KEY (UserId) REFERENCES Usuarios(Id),
			CONSTRAINT FK_UserGames_Games FOREIGN KEY (GameId) REFERENCES Games(Id)
		);
		GO

		CREATE TABLE GamePlatforms (
			Id INT PRIMARY KEY IDENTITY (1,1),
			GameId INT NOT NULL,
			PlatformId INT NOT NULL,
			CONSTRAINT FK_GamePlatform_Games FOREIGN KEY (GameId) REFERENCES Games(Id),
			CONSTRAINT FK_GamePlatform_Platforms FOREIGN KEY (PlatformId) REFERENCES Platforms(Id)
		);
		GO

		CREATE TABLE GameGenres (
			Id INT PRIMARY KEY IDENTITY (1,1),
			GameId INT NOT NULL,
			GenreId INT NOT NULL,
			CONSTRAINT FK_GameGenres_Games FOREIGN KEY (GameId) REFERENCES Games(Id),
			CONSTRAINT FK_GameGenres_Genres FOREIGN KEY (GenreId) REFERENCES Genres(Id)
		);
		GO
        */
    }
}
