import java.util.ArrayList;
import java.util.List;

public class ContainerShip {
    private String name;
    private double maxSpeed;
    private int maxContainerCount;
    private double maxWeight;
    private List<Container> containers;

    public ContainerShip(String name, double maxSpeed, int maxContainerCount, double maxWeightInTons) {
        this.name = name;
        this.maxSpeed = maxSpeed;
        this.maxContainerCount = maxContainerCount;
        this.maxWeight = maxWeightInTons;
        this.containers = new ArrayList<>();
    }

    public String getName() {
        return name;
    }

    private boolean canLoad(Container container) {
        if (containers.size() >= maxContainerCount) {
            return false;
        }
        double totalWeightKg = containers.stream().mapToDouble(Container::getTotalWeight).sum();
        totalWeightKg += container.getTotalWeight();
        double maxWeightKg = maxWeight * 1000.0;
        return totalWeightKg <= maxWeightKg;
    }

    public boolean loadContainer(Container container) {
        if (!canLoad(container)) {
            System.out.println("Nie można załadować " + container.getSerialNumber() + " na " + name);
            return false;
        }
        containers.add(container);
        System.out.println("Załadowano " + container.getSerialNumber() + " na " + name);
        return true;
    }

    public void loadContainers(List<Container> containerList) {
        for (Container c : containerList) {
            loadContainer(c);
        }
    }

    public boolean removeContainer(String serialNumber) {
        return containers.removeIf(c -> c.getSerialNumber().equals(serialNumber));
    }

    public boolean replaceContainer(String oldSerialNumber, Container newContainer) {
        for (int i = 0; i < containers.size(); i++) {
            if (containers.get(i).getSerialNumber().equals(oldSerialNumber)) {
                Container removed = containers.remove(i);
                if (canLoad(newContainer)) {
                    containers.add(i, newContainer);
                    System.out.println("Zastąpiono " + oldSerialNumber + " kontenerem " + newContainer.getSerialNumber());
                    return true;
                } else {
                    containers.add(i, removed);
                    System.out.println("Nie można zastąpić " + oldSerialNumber);
                    return false;
                }
            }
        }
        return false;
    }

    public boolean transferContainer(String serialNumber, ContainerShip targetShip) {
        for (Container c : containers) {
            if (c.getSerialNumber().equals(serialNumber)) {
                if (targetShip.loadContainer(c)) {
                    removeContainer(serialNumber);
                    System.out.println("Przeniesiono " + serialNumber + " z " + this.name + " na " + targetShip.getName());
                    return true;
                } else {
                    System.out.println("Nie można przenieść " + serialNumber);
                    return false;
                }
            }
        }
        System.out.println("Nie znaleziono " + serialNumber + " na " + name);
        return false;
    }

    public void printInfo() {
        System.out.println("=== " + name + " ===");
        System.out.println("Prędkość: " + maxSpeed);
        System.out.println("Max kontenery: " + maxContainerCount);
        System.out.println("Max waga (t): " + maxWeight);
        System.out.println("Obecna liczba: " + containers.size());
        double totalWeightKg = containers.stream().mapToDouble(Container::getTotalWeight).sum();
        System.out.println("Obecna waga (kg): " + totalWeightKg);
        for (Container c : containers) {
            System.out.println("- " + c.getSerialNumber() + " | " + c.getTotalWeight() + " kg");
        }
    }
}
