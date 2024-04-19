namespace AnimalsDB.Animal;

public class Animal
{
    public int IdAnimal { get; private set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string Area { get; set; }
    
    public void setIdAnimal(int idAnimal)
    {
        IdAnimal = idAnimal;
    }
}