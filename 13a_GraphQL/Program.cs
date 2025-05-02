using GraphQLBooks;
using HotChocolate.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSingleton<DataStore>()
    .AddGraphQLServer()
    .AddDocumentFromFile("./schema.graphql")
    .BindRuntimeType<Query>()
    .BindRuntimeType<Mutation>()
    .BindRuntimeType<Book>("Book")
    .BindRuntimeType<Author>("Author")
    .BindRuntimeType<ErrorMessage>("ErrorMessage")
    .BindRuntimeType<SuccessMessage>("SuccessMessage");

var app = builder.Build();

app.UseRouting();
app.MapGraphQL();

app.Run();