enum CarType {
	Tesla,
	BMW,
	Toyota,
	Nissan,
	Audi
}

interface ICar {
	string Brand { get; }
	int Seats { get; }
	string GetDescription();
}

interface IElectric {
	int BatteryCapacity { get; }
}

interface IGas {
	int TankCapacity { get; }
}

interface IMechanical {
	int Gears { get; }
}

interface IAutomatical {
	int Speeds { get; }
}

abstract class ACar : ICar {
	public string Brand { get; protected set; }
	public int Seats { get; protected set; }
	public string TransmissionName { get; protected set; }
	public string OnboardSystem { get; protected set; }

	protected ACar(string brand, int seats, string transmissionName, string onboardSystem) {
		Brand = brand;
		Seats = seats;
		TransmissionName = transmissionName;
		OnboardSystem = onboardSystem;
	}

	public virtual string GetDescription() {
		return Brand + ": " +
			GetEngineDescription() + ", " +
			GetTransmissionDescription() + ", " +
			Seats + " мест, " +
			OnboardSystem + " на борту";
	}

	protected virtual string GetEngineDescription() {
		return "обычный автомобиль";
	}

	protected virtual string GetTransmissionDescription() {
		return TransmissionName;
	}
}

abstract class AutomaticCar : ACar, IAutomatical {
	public int Speeds { get; protected set; }

	protected AutomaticCar(string brand, int seats, int speeds, string onboardSystem)
		: base(brand, seats, "автоматическая коробка передач", onboardSystem) {
		Speeds = speeds;
	}

	protected override string GetTransmissionDescription() {
		return TransmissionName + " (" + Speeds + "-ступ.)";
	}
}

abstract class MechanicalCar : ACar, IMechanical {
	public int Gears { get; protected set; }

	protected MechanicalCar(string brand, int seats, int gears, string onboardSystem)
		: base(brand, seats, "механическая коробка передач", onboardSystem) {
		Gears = gears;
	}

	protected override string GetTransmissionDescription() {
		return TransmissionName + " (" + Gears + "-ступ.)";
	}
}

class TeslaCar : AutomaticCar, IElectric {
	public int BatteryCapacity { get; private set; }

	public TeslaCar() : base("Tesla", 5, 1, "Android Auto") {
		BatteryCapacity = 85;
	}

	protected override string GetEngineDescription() {
		return "электромобиль (батарея " + BatteryCapacity + " kWh)";
	}
}

class BmwCar : AutomaticCar, IGas {
	public int TankCapacity { get; private set; }

	public BmwCar() : base("BMW", 5, 8, "iDrive") {
		TankCapacity = 60;
	}

	protected override string GetEngineDescription() {
		return "бензиновый автомобиль (бак " + TankCapacity + " л)";
	}
}

class ToyotaCar : MechanicalCar, IGas {
	public int TankCapacity { get; private set; }

	public ToyotaCar() : base("Toyota", 5, 6, "CarPlay") {
		TankCapacity = 55;
	}

	protected override string GetEngineDescription() {
		return "бензиновый автомобиль (бак " + TankCapacity + " л)";
	}
}

class NissanCar : AutomaticCar, IElectric {
	public int BatteryCapacity { get; private set; }

	public NissanCar() : base("Nissan", 5, 1, "Nissan Connect") {
		BatteryCapacity = 62;
	}

	protected override string GetEngineDescription() {
		return "электромобиль (батарея " + BatteryCapacity + " kWh)";
	}
}

class AudiCar : MechanicalCar, IGas {
	public int TankCapacity { get; private set; }

	public AudiCar() : base("Audi", 4, 6, "MMI") {
		TankCapacity = 58;
	}

	protected override string GetEngineDescription() {
		return "бензиновый автомобиль (бак " + TankCapacity + " л)";
	}
}

static class CarFactory {
	public static ICar CreateCar(CarType type) {
		switch (type) {
			case CarType.Tesla:
				return new TeslaCar();
			case CarType.BMW:
				return new BmwCar();
			case CarType.Toyota:
				return new ToyotaCar();
			case CarType.Nissan:
				return new NissanCar();
			case CarType.Audi:
				return new AudiCar();
			default:
				throw new ArgumentOutOfRangeException(nameof(type), type, "Неизвестный тип автомобиля");
		}
	}
}

class CarsProgram {
	static string ReadInput(string prompt) {
		Console.Write(prompt);
		return Console.ReadLine() ?? string.Empty;
	}

	static bool TryParseCarType(string brand, out CarType type) {
		if (string.Equals(brand, "tesla", StringComparison.OrdinalIgnoreCase)) {
			type = CarType.Tesla;
			return true;
		}

		if (string.Equals(brand, "bmw", StringComparison.OrdinalIgnoreCase)) {
			type = CarType.BMW;
			return true;
		}

		if (string.Equals(brand, "toyota", StringComparison.OrdinalIgnoreCase)) {
			type = CarType.Toyota;
			return true;
		}

		if (string.Equals(brand, "nissan", StringComparison.OrdinalIgnoreCase)) {
			type = CarType.Nissan;
			return true;
		}

		if (string.Equals(brand, "audi", StringComparison.OrdinalIgnoreCase)) {
			type = CarType.Audi;
			return true;
		}

		type = default;
		return false;
	}

	static void Main() {
		while (true) {
			string input = ReadInput("Введите марку автомобиля или done для остановки ввода: ").Trim();

			if (string.Equals(input, "done", StringComparison.OrdinalIgnoreCase)) {
				break;
			}

			if (!TryParseCarType(input, out CarType type)) {
				Console.WriteLine("Неизвестная марка. Доступно: Tesla, BMW, Toyota, Nissan, Audi.");
				Console.WriteLine();
				continue;
			}

			ICar car = CarFactory.CreateCar(type);
			Console.WriteLine(car.GetDescription());
			Console.WriteLine();
		}
	}
}
