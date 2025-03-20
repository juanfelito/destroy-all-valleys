[System.Serializable]
public class GridProperty {
    public GridCoordinate coordinate;
    public GridBoolProperty property;
    public bool value = false;

    public GridProperty(GridCoordinate coordinate, GridBoolProperty property, bool value) {
        this.coordinate = coordinate;
        this.property = property;
        this.value = value;
    }
}
