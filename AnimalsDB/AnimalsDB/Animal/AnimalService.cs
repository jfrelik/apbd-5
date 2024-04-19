using System.Data.SqlClient;

namespace AnimalsDB.Animal;
using Microsoft.Extensions.Configuration;

public class AnimalService : IAnimalService
{
    private readonly IConfiguration _configuration;
    
    public AnimalService(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public IEnumerable<Animal> GetAnimals(string orderBy)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        
        string[] validColumns = ["Name", "Description", "Category", "Area"];
        if (!validColumns.Contains(orderBy))
            throw new ArgumentException("Invalid order by column");

        var query = $"SELECT * FROM Animal ORDER BY {orderBy} ASC";
        
        using var cmd = new SqlCommand(query, con);
        
        using var reader = cmd.ExecuteReader();
        var animals = new List<Animal>();
        while (reader.Read())
        {
            var newAnimal = new Animal
            {
                Name = reader.GetString(1),
                Description = reader.GetString(2),
                Category = reader.GetString(3),
                Area = reader.GetString(4)
            };
            
            newAnimal.setIdAnimal(reader.GetInt32(0));
            
            animals.Add(newAnimal);
        }

        return animals;
    }

    public Animal AddAnimal(Animal animal)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        
        const string maxIdQuery = "SELECT MAX(IdAnimal) FROM Animal";
        using var cmdMaxId = new SqlCommand(maxIdQuery, con);
        var maxId = (int)cmdMaxId.ExecuteScalar() + 1;
        
        const string query = "INSERT INTO Animal (IdAnimal, Name, Description, Category, Area) VALUES (@Id, @Name, @Description, @Category, @Area)";
        
        using var cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@Id", maxId);
        cmd.Parameters.AddWithValue("@Name", animal.Name);
        cmd.Parameters.AddWithValue("@Description", animal.Description);
        cmd.Parameters.AddWithValue("@Category", animal.Category);
        cmd.Parameters.AddWithValue("@Area", animal.Area);
        
        cmd.ExecuteNonQuery();
        
        animal.setIdAnimal(maxId);
        
        return animal;
    }

    public Animal? UpdateAnimal(Animal animal)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        
        const string query = "UPDATE Animal SET Name = @Name, Description = @Description, Category = @Category, Area = @Area WHERE IdAnimal = @Id";
        
        using var cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@Id", animal.IdAnimal);
        cmd.Parameters.AddWithValue("@Name", animal.Name);
        cmd.Parameters.AddWithValue("@Description", animal.Description);
        cmd.Parameters.AddWithValue("@Category", animal.Category);
        cmd.Parameters.AddWithValue("@Area", animal.Area);
        
        var rowsAffected = cmd.ExecuteNonQuery();
        
        return rowsAffected == 0 ? null : animal;
    }

    public int? DeleteAnimal(int id)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        
        const string query = "DELETE FROM Animal WHERE IdAnimal = @IdAnimal";
        
        using var cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@IdAnimal", id);
        
        var rowsAffected = cmd.ExecuteNonQuery();
        if (rowsAffected == 0)
            return null;

        return rowsAffected;
    }
}