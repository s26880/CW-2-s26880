// ReeferContainer.java
public class ReeferContainer extends Container {
    private double temperature;
    private ProductType productType;

    public ReeferContainer(double tareWeight, double maxCapacity, ProductType productType, double initialTemperature) throws HazardOperationException {
        super("C", tareWeight, maxCapacity);
        this.productType = productType;
        setTemperature(initialTemperature);
    }

    public void setTemperature(double newTemperature) throws HazardOperationException {
        double required = productType.getRequiredMinTemperature();
        if (newTemperature < required) {
            throw new HazardOperationException("Temperatura zbyt niska dla " + productType.name());
        }
        this.temperature = newTemperature;
    }

    @Override
    public void loadCargo(double mass) throws OverfillException {
        if (currentLoad + mass > maxCapacity) {
            throw new OverfillException("Przekroczono pojemność: " + getSerialNumber());
        }
        currentLoad += mass;
    }

    @Override
    public void unloadCargo() {
        currentLoad = 0.0;
    }

    public double getTemperature() {
        return temperature;
    }

    public ProductType getProductType() {
        return productType;
    }
}
