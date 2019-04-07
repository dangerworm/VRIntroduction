using UnityEngine;

public class InitialiseBuildings : MonoBehaviour
{
    public Transform BuildingTransform;
    public Transform FloorTransform;

    public int NumberOfBuildings;

    // Use this for initialization
    void Start()
    {
        var xRange = new RangeAttribute(-FloorTransform.localScale.x / 2, FloorTransform.localScale.x / 2);
        var yRange = new RangeAttribute(3.0f, 100);
        var zRange = new RangeAttribute(-FloorTransform.localScale.z / 2, FloorTransform.localScale.z / 2);

        for (var i = 0; i < NumberOfBuildings; i++)
        {
            var xPosition = Random.Range(xRange.min * FloorTransform.localScale.x, xRange.max * FloorTransform.localScale.x);
            var zPosition = Random.Range(zRange.min * FloorTransform.localScale.z, zRange.max * FloorTransform.localScale.z);

            var xScale = Random.Range(xRange.min, xRange.max);
            var yScale = Random.Range(yRange.min, yRange.max);
            var zScale = Random.Range(zRange.min, zRange.max);

            var position = new Vector3(xPosition, yScale / 2, zPosition);
            var scale = new Vector3(xScale, yScale, zScale);

            var newBuilding = Instantiate(BuildingTransform, position, Quaternion.identity);
            newBuilding.localScale = scale;
        }
    }
}
