class Product {
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

static class CollectionUtils {
	public static List<T> Distinct<T>(List<T> source) {
		HashSet<T> seen = new HashSet<T>();
		List<T> result = new List<T>();

		foreach (T item in source) {
			if (seen.Add(item)) {
				result.Add(item);
			}
		}

		return result;
	}

	public static Dictionary<TKey, List<TValue>> GroupBy<TValue, TKey>(
		List<TValue> source,
		Func<TValue, TKey> keySelector) where TKey : notnull {
		Dictionary<TKey, List<TValue>> result = new Dictionary<TKey, List<TValue>>();

		foreach (TValue item in source) {
			TKey key = keySelector(item);

			if (!result.TryGetValue(key, out List<TValue>? bucket)) {
				bucket = new List<TValue>();
				result[key] = bucket;
			}

			bucket.Add(item);
		}

		return result;
	}

	public static Dictionary<TKey, TValue> Merge<TKey, TValue>(
		Dictionary<TKey, TValue> first,
		Dictionary<TKey, TValue> second,
		Func<TValue, TValue, TValue> conflictResolver) where TKey : notnull {
		Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();

		foreach (KeyValuePair<TKey, TValue> pair in first) {
			result[pair.Key] = pair.Value;
		}

		foreach (KeyValuePair<TKey, TValue> pair in second) {
			if (result.TryGetValue(pair.Key, out TValue? existing)) {
				result[pair.Key] = conflictResolver(existing, pair.Value);
			} else {
				result[pair.Key] = pair.Value;
			}
		}

		return result;
	}

	public static T MaxBy<T, TKey>(List<T> source, Func<T, TKey> selector)
		where TKey : IComparable<TKey> {
		if (source.Count == 0) {
			throw new InvalidOperationException("Sequence contains no elements");
		}

		T bestItem = source[0];
		TKey bestKey = selector(bestItem);

		for (int i = 1; i < source.Count; i++) {
			T currentItem = source[i];
			TKey currentKey = selector(currentItem);

			if (currentKey.CompareTo(bestKey) > 0) {
				bestItem = currentItem;
				bestKey = currentKey;
			}
		}

		return bestItem;
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

	static void PrintDictionary<TKey, TValue>(string title, Dictionary<TKey, TValue> dict) where TKey : notnull {
		Console.WriteLine(title);
		foreach (KeyValuePair<TKey, TValue> pair in dict) {
			Console.WriteLine(pair.Key + ": " + pair.Value);
		}
		Console.WriteLine();
	}

	static void Main() {
		List<int> nums = new List<int> { 1, 2, 2, 3, 1, 4 };
		PrintList("Distinct ints:", CollectionUtils.Distinct(nums));

		List<string> words = new List<string> { "one", "two", "three", "two", "four" };
		PrintList("Distinct strings:", CollectionUtils.Distinct(words));

		Dictionary<int, List<string>> grouped = CollectionUtils.GroupBy(words, w => w.Length);
		Console.WriteLine("GroupBy length:");
		foreach (KeyValuePair<int, List<string>> pair in grouped) {
			Console.WriteLine(pair.Key + ": " + string.Join(", ", pair.Value));
		}
		Console.WriteLine();

		Dictionary<string, int> first = new Dictionary<string, int>();
		first["apple"] = 2;
		first["orange"] = 1;

		Dictionary<string, int> second = new Dictionary<string, int>();
		second["apple"] = 3;
		second["banana"] = 4;

		Dictionary<string, int> merged = CollectionUtils.Merge(first, second, (a, b) => a + b);
		PrintDictionary("Merged counts:", merged);

		List<Product> products = new List<Product> {
			new Product(10, "Phone", 800),
			new Product(11, "Laptop", 2500),
			new Product(12, "Tablet", 1400)
		};

		Product maxProduct = CollectionUtils.MaxBy(products, p => p.Price);
		Console.WriteLine("Max product: " + maxProduct);
	}
}
