using AutoFixture;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;
using ValueBlue.Movies.Application.Exceptions;
using ValueBlue.Movies.Domain.Models;
using ValueBlue.Movies.Infrastructure.ServiceAgent;
using ValueBlue.Movies.Tests.Helpers;
using Xunit;

namespace ValueBlue.Movies.Tests
{
    public class OmdbServiceAgentUnitTests
    {
        IOptions<OmdbServiceConfig> options;
        Fixture fixture;
        ILogger<OmdbServiceAgent> fakelogger;

        public OmdbServiceAgentUnitTests()
        {
            options = A.Fake<IOptions<OmdbServiceConfig>>();
            fixture = new Fixture();
            fakelogger = A.Fake<ILogger<OmdbServiceAgent>>();
        }

        [Fact]
        public async Task SearchMovieByTitle_With_Valid_Input_Returns_A_Movie()
        {
            //Arrange
            var title = "A_Valid_MovieTitle";
            var movie = fixture.Create<Movie>();
            var httpClient = CreateAFakeHttpClientWithResponse(HttpStatusCode.OK, JsonSerializer.Serialize(movie));
            var sut = new OmdbServiceAgent(httpClient, options, fakelogger);

            //Act
            var result = await sut.SearchMovieByTitle(title);
            
            //Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(movie);
        }

        [Theory]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.ServiceUnavailable)]
        [InlineData(HttpStatusCode.Unauthorized)]
        public void SearchMovieByTitle_With_InValid_Input_Throws_A_Custom_Exception(System.Net.HttpStatusCode statusCode)
        {
            //Arrange
            var title = "A_NonExisting_MovieTitle";
            var httpClient = CreateAFakeHttpClientWithResponse(statusCode);
            var sut = new OmdbServiceAgent(httpClient, options, fakelogger);

            //Act
            Func<Task> act = async () => await sut.SearchMovieByTitle(title);

            //Assert
            act.Should().ThrowAsync<MovieSearchFailedException>();
        }

        private HttpClient CreateAFakeHttpClientWithResponse(System.Net.HttpStatusCode statusCode, object? content = null)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response.StatusCode = statusCode;

            if (content != null)
            {
                var httpResponseContent = GetStreamContent(content);
                response.Content = httpResponseContent;
            }

            var handler = A.Fake<FakeableHttpMessageHandler>();
            A.CallTo(() => handler.FakeSendAsync(A<HttpRequestMessage>.Ignored, A<CancellationToken>.Ignored))
                .Returns(response);

            var client = new HttpClient(handler) { BaseAddress = new Uri("http://url") };
            return client;
        }

        private static StreamContent GetStreamContent(object content)
        {
            var memStream = new MemoryStream();

            var sw = new StreamWriter(memStream);
            sw.Write(content);
            sw.Flush();
            memStream.Position = 0;

            var httpContent = new StreamContent(memStream);
            return httpContent;
        }
    }

}
