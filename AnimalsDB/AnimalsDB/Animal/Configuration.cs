namespace AnimalsDB.Animal;

public static class Configuration
{
    public static void RegisterEndpointsForAnimals(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/animals/{orderBy}", (IAnimalService service, String orderBy) =>
        {
            var result = service.GetAnimals(orderBy);

            return TypedResults.Ok(result);
        });
        
        app.MapPost("api/v1/animals", (IAnimalService service, Animal animal) =>
        {
            service.AddAnimal(animal);

            return TypedResults.Created("GetAnimals", animal);
        });

        app.MapPut("api/v1/animals/{id:int}", (IAnimalService service, Animal animal, int id) =>
        {
            animal.setIdAnimal(id);
            var updatedAnimal = service.UpdateAnimal(animal);

            return updatedAnimal == null ? Results.NotFound() : TypedResults.Ok(updatedAnimal);
        });

        app.MapDelete("api/v1/animals/{id:int}", (IAnimalService service, int id) =>
        {
            var result = service.DeleteAnimal(id);
            
            if (result == null)
                return Results.NoContent();

            return Results.Ok();
        });
    }
}