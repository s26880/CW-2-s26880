// ProductType.java
import java.util.Arrays;
public enum ProductType {
    BANANAS(13.3),
    CHOCOLATE(18.0),
    FISH(2.0),
    MEAT(-15.0),
    ICE_CREAM(-18.0),
    FROZEN_PIZZA(-30.0),
    CHEESE(7.2),
    SAUSAGES(5.0),
    BUTTER(20.5),
    EGGS(19.0);

    private final double requiredMinTemperature;

    ProductType(double requiredMinTemperature) {
        this.requiredMinTemperature = requiredMinTemperature;
    }

    public double getRequiredMinTemperature() {
        return requiredMinTemperature;
    }
}




