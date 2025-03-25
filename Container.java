// Container.java
public abstract class Container {
    private static int globalCounter = 1;
    protected String serialNumber;
    protected double tareWeight;
    protected double maxCapacity;
    protected double currentLoad;

    public Container(String typePrefix, double tareWeight, double maxCapacity) {
        this.serialNumber = generateSerialNumber(typePrefix);
        this.tareWeight = tareWeight;
        this.maxCapacity = maxCapacity;
        this.currentLoad = 0.0;
    }

    private String generateSerialNumber(String typePrefix) {
        return "KON-" + typePrefix + "-" + (globalCounter++);
    }

    public String getSerialNumber() {
        return serialNumber;
    }

    public double getTareWeight() {
        return tareWeight;
    }

    public double getMaxCapacity() {
        return maxCapacity;
    }

    public double getCurrentLoad() {
        return currentLoad;
    }

    public abstract void loadCargo(double mass) throws OverfillException, HazardOperationException;

    public abstract void unloadCargo();

    public double getTotalWeight() {
        return tareWeight + currentLoad;
    }
}
