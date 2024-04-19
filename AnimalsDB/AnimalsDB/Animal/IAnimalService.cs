namespace AnimalsDB.Animal;

public interface IAnimalService
{
        IEnumerable<Animal> GetAnimals(string orderBy);
        
        Animal AddAnimal(Animal animal);
        
        Animal? UpdateAnimal(Animal animal);
        
        int? DeleteAnimal(int id);   
}