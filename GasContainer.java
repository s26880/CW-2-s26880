// GasContainer.java
public class GasContainer extends Container implements IHazardNotifier {
    private double pressure;

    public GasContainer(double tareWeight, double maxCapacity, double pressure) {
        super("G", tareWeight, maxCapacity);
        this.pressure = pressure;
    }

    @Override
    public void notifyHazard(String containerNumber, String message) {
        System.err.println("HAZARD in " + containerNumber + ": " + message);
    }

    @Override
    public void loadCargo(double mass) throws OverfillException, HazardOperationException {
        if (currentLoad + mass > maxCapacity) {
            notifyHazard(serialNumber, "Przekroczono dopuszczalną ładowność.");
            throw new OverfillException("Przekroczono pojemność: " + getSerialNumber());
        }
        currentLoad += mass;
    }

    @Override
    public void unloadCargo() {
        currentLoad = currentLoad * 0.05;
    }

    public double getPressure() {
        return pressure;
    }

    public void setPressure(double pressure) {
        this.pressure = pressure;
    }
}
