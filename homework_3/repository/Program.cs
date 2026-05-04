interface IEntity {
	int Id { get; }
}

class Repository<T> where T : IEntity {
	readonly Dictionary<int, T> items = new Dictionary<int, T>();

	public int Count {
		get { return items.Count; }
	}

	public void Add(T item) {
		if (items.ContainsKey(item.Id)) {
			throw new InvalidOperationException("Item with the same Id already exists");
		}

		items[item.Id] = item;
	}

	public bool Remove(int id) {
		return items.Remove(id);
	}

	public T? GetById(int id) {
		if (items.TryGetValue(id, out T? value)) {
			return value;
		}

		return default;
	}

	public IReadOnlyList<T> GetAll() {
		return new List<T>(items.Values);
	}

	public IReadOnlyList<T> Find(Predicate<T> predicate) {
		List<T> result = new List<T>();

		foreach (T item in items.Values) {
			if (predicate(item)) {
				result.Add(item);
			}
		}

		return result;
	}
}

class Product : IEntity {
	public int Id { get; private set; }
	public string Name { get; private set; }
	public decimal Price { get; private set; }

	public Product(int id, string name, decimal price) {
		Id = id;
		Name = name;
		Price = price;
	}

	public override string ToString() {
		return Id + ": " + Name + " (" + Price + ")";
	}
}

class User : IEntity {
	public int Id { get; private set; }
	public string Name { get; private set; }
	public int Age { get; private set; }

	public User(int id, string name, int age) {
		Id = id;
		Name = name;
		Age = age;
	}

	public override string ToString() {
		return Id + ": " + Name + " (" + Age + ")";
	}
}

class Program {
	static void PrintList<T>(string title, IReadOnlyList<T> list) {
		Console.WriteLine(title);
		foreach (T item in list) {
			Console.WriteLine(item);
		}
		Console.WriteLine();
	}

	static void Main() {
		Repository<Product> productRepo = new Repository<Product>();
		productRepo.Add(new Product(1, "Keyboard", 1200));
		productRepo.Add(new Product(2, "Mouse", 700));
		productRepo.Add(new Product(3, "Monitor", 9500));

		Console.WriteLine("Product by id 2: " + productRepo.GetById(2));
		PrintList("Products over 1000:", productRepo.Find(p => p.Price > 1000));

		try {
			productRepo.Add(new Product(2, "Duplicate", 1));
		} catch (InvalidOperationException e) {
			Console.WriteLine("Duplicate add: " + e.Message);
		}
		Console.WriteLine();

		Repository<User> userRepo = new Repository<User>();
		userRepo.Add(new User(1, "Alice", 22));
		userRepo.Add(new User(2, "Bob", 31));
		Console.WriteLine("User by id 1: " + userRepo.GetById(1));
		Console.WriteLine("Users count: " + userRepo.Count);
		Console.WriteLine();
	}
}
