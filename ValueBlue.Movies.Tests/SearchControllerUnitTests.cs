using FakeItEasy;
using ValueBlue.Movies.Application.Interfaces;
using Xunit;
using ValueBlue.Movies.WebApi.Controllers;
using AutoFixture;
using ValueBlue.Movies.Domain.Models;
using ValueBlue.Movies.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ValueBlue.Movies.Application.Exceptions;
using Microsoft.Extensions.Logging;

namespace ValueBlue.Movies.Tests
{
    public class SearchControllerUnitTests
    {
        IMovieSearchService movieSearchService;
        IRepositoryService repositoryService;
        Fixture fixture;
        ILogger<SearchController> fakelogger;

        public SearchControllerUnitTests()
        {
            movieSearchService = A.Fake<IMovieSearchService>();
            repositoryService = A.Fake<IRepositoryService>();
            fixture = new Fixture();
            fakelogger = A.Fake<ILogger<SearchController>>();
        }

        [Fact]
        public async Task Search_Returns_200_With_A_ValidMovie()
        {
            //Arrange
            var title = "A_Valid_MovieTitle";
            var sut = new SearchController(movieSearchService, repositoryService, fakelogger);
            sut.ControllerContext = new ControllerContext() { HttpContext = new DefaultHttpContext() };
            sut.ControllerContext.HttpContext.Connection.RemoteIpAddress = A.Fake<IPAddress>(); //new System.Net.IPAddress(0x2414188f);

            var movie = fixture.Create<Movie>();
            A.CallTo(() => movieSearchService.GetMovieByTitle(A<string>._)).Returns(movie);
            A.CallTo(() => repositoryService.CreateEntity(A<SearchRequest>._)).DoesNothing();

            //Act
            var response = await sut.GetMovieByTitle(title);
            
            //Assert
            Assert.NotNull(response.Result);
            var result = response.Result as ObjectResult;
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(result.StatusCode, 200);
            result.Value.Should().BeEquivalentTo(movie);
        }

        [Fact]
        public async Task Search_Returns_200_With_A_ValidMovie_When_MovieSearchSucceeded_But_SaveFailed()
        {
            //Arrange
            var title = "A_Valid_MovieTitle";
            var sut = new SearchController(movieSearchService, repositoryService, fakelogger);
            sut.ControllerContext = new ControllerContext() { HttpContext = new DefaultHttpContext() };
            sut.ControllerContext.HttpContext.Connection.RemoteIpAddress = A.Fake<IPAddress>();

            var movie = fixture.Create<Movie>();
            A.CallTo(() => movieSearchService.GetMovieByTitle(A<string>._)).Returns(movie);
            A.CallTo(() => repositoryService.CreateEntity(A<SearchRequest>._)).Throws<SaveSearchRequestFailedException>();

            //Act
            var response = await sut.GetMovieByTitle(title);

            //Assert
            Assert.NotNull(response.Result);
            var result = response.Result as ObjectResult;
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(result.StatusCode, 200);
            result.Value.Should().BeEquivalentTo(movie);
        }

        [Fact]
        public async Task Search_Returns_404_When_MovieSearchFailed_With_An_Exception()
        {
            //Arrange
            var title = "A_NonExisting_MovieTitle";
            var sut = new SearchController(movieSearchService, repositoryService, fakelogger);
            A.CallTo(() => movieSearchService.GetMovieByTitle(A<string>._)).Throws<MovieSearchFailedException>();

            //Act
            var response = await sut.GetMovieByTitle(title);

            //Assert
            Assert.NotNull(response.Result);
            var result = response.Result as ObjectResult;
            Assert.NotNull(result);
            Assert.Equal(result.StatusCode, 404);
        }

        [Fact]
        public async Task Search_Returns_404_When_MovieSearchService_Returns_Null()
        {
            //Arrange
            var title = "A_NonExisting_MovieTitle";
            Movie nullReturn = null;
            var sut = new SearchController(movieSearchService, repositoryService, fakelogger);
            A.CallTo(() => movieSearchService.GetMovieByTitle(A<string>._)).Returns(nullReturn);

            //Act
            var response = await sut.GetMovieByTitle(title);

            //Assert
            Assert.NotNull(response.Result);
            var result = response.Result as ObjectResult;
            Assert.NotNull(result);
            Assert.Equal(result.StatusCode, 404);
        }
    }
}
