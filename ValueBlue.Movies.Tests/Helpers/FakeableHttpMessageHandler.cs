namespace ValueBlue.Movies.Tests.Helpers
{
    public abstract class FakeableHttpMessageHandler : HttpMessageHandler
    {
        protected sealed override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, CancellationToken cancellationToken
            ) => FakeSendAsync(request, cancellationToken);

        public abstract Task<HttpResponseMessage> FakeSendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken);
    }
}
