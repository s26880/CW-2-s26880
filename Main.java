// Main.java

public class Main {
    public static void main(String[] args) {
        try {
            LiquidContainer milkContainer = new LiquidContainer(200.0, 1000.0, false);
            LiquidContainer fuelContainer = new LiquidContainer(300.0, 2000.0, true);
            GasContainer heliumContainer = new GasContainer(150.0, 500.0, 10.0);
            ReeferContainer bananaContainer = new ReeferContainer(250.0, 800.0, ProductType.BANANAS, 14.0);
            milkContainer.loadCargo(500.0);
            fuelContainer.loadCargo(900.0);
            heliumContainer.loadCargo(400.0);
            bananaContainer.loadCargo(600.0);
            ContainerShip shipA = new ContainerShip("Atlantic Express", 25.0, 3, 10.0);
            ContainerShip shipB = new ContainerShip("Pacific Runner", 20.0, 5, 20.0);
            shipA.loadContainer(milkContainer);
            shipA.loadContainer(fuelContainer);
            shipA.loadContainer(heliumContainer);
            shipA.loadContainer(bananaContainer);
            shipA.printInfo();
            shipB.printInfo();
            shipA.transferContainer(heliumContainer.getSerialNumber(), shipB);
            shipB.loadContainer(bananaContainer);
            shipA.printInfo();
            shipB.printInfo();
            shipA.replaceContainer(fuelContainer.getSerialNumber(), bananaContainer);
        } catch (Exception e) {
            System.err.println("Błąd: " + e.getMessage());
        }
    }
}
