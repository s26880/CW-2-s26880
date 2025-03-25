// LiquidContainer.java
public class LiquidContainer extends Container implements IHazardNotifier {
    private boolean isDangerous;

    public LiquidContainer(double tareWeight, double maxCapacity, boolean isDangerous) {
        super("L", tareWeight, maxCapacity);
        this.isDangerous = isDangerous;
    }

    @Override
    public void notifyHazard(String containerNumber, String message) {
        System.err.println("HAZARD in " + containerNumber + ": " + message);
    }

    @Override
    public void loadCargo(double mass) throws OverfillException, HazardOperationException {
        double limit = isDangerous ? 0.5 * maxCapacity : 0.9 * maxCapacity;
        if (currentLoad + mass > limit) {
            notifyHazard(serialNumber, "Przekroczono dopuszczalny limit załadunku.");
            throw new HazardOperationException("Niebezpieczna operacja załadunku: " + getSerialNumber());
        }
        if (currentLoad + mass > maxCapacity) {
            throw new OverfillException("Przekroczono pojemność: " + getSerialNumber());
        }
        currentLoad += mass;
    }

    @Override
    public void unloadCargo() {
        currentLoad = 0.0;
    }

    public boolean isDangerous() {
        return isDangerous;
    }
}
