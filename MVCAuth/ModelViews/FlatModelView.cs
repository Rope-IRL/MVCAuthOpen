namespace MVCAuth.ModelViews;

public class FlatModelView

{
    public IEnumerable<Flat> Flats { get; set; }
    
    public IEnumerable<LandLordsAdditionalInfo> LandlordsInfo { get; set; }
}