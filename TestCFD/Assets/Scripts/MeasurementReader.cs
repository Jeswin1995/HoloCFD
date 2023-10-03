using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class MeasurementReader : MonoBehaviour
{
    [SerializeField] mqttReceiver mqttReceiver;
    [SerializeField] InspectionPlanReader inspectionPlanReader;
    public MeasurementResult MeasurementResult { get; private set; }
    public delegate void MeasurementReceived();
    public event MeasurementReceived OnMeasurementReceived;

    void OnEnable()
    {
        mqttReceiver.OnMessageArrived += LoadMeasurement;
    }
    void OnDisable()
    {
        mqttReceiver.OnMessageArrived -= LoadMeasurement;
    }
    public void LoadMeasurement(string xml)
    {
        Debug.Log("MQTT OnMessageArrived event fired. The message is = " + xml);
        XmlSerializer serializer = new XmlSerializer(typeof(MeasurementResult));
        MeasurementResult measurementResult;

        using (TextReader reader = new StringReader(xml))
        {
            measurementResult = (MeasurementResult)serializer.Deserialize(reader);
        }
        Debug.Log("MQTT: New XML Inspection Plan deserialized.");
        //Debug.Log("Deserialized. Test2: " + mqttData.AllMeasurements.Measurements[0].Type);
        //TextWriter writer = new StreamWriter("C:/Users/admin/Desktop/protocol3.xml");
        //serializer.Serialize(writer, mqttData);
        //writer.Close();
        MeasurementResult = measurementResult;

        /// TODO: Check if received measurement result id == activemeasurementId
        /// if not: open prompt and ask if you want to switch to received measurement
        

        // Get id of received measurement result
        int idMeasurementResult = measurementResult.Id;      
        // Update measurement results in inspection plan
        foreach (var measurement in inspectionPlanReader.MqttData.AllMeasurements.Measurements.Where(i => i.Id == idMeasurementResult))
        {
            measurement.Step = measurementResult.Step;
            measurement.Gap = measurementResult.Gap;
            measurement.Take = measurementResult.Take;
        }
        // Run measurement received event
        if (OnMeasurementReceived != null)
            OnMeasurementReceived();
    }
}

[XmlRoot("measurement")]
public class MeasurementResult
{
    public MeasurementResult() { }
    public MeasurementResult(Measurement measurement)
    {
        Id = measurement.Id;
        OrderNr = measurement.OrderNr;
        PieceNr = measurement.PieceNr;
        Type = measurement.Type;
        Position = measurement.Position;
        Step = measurement.Step;
        Gap = measurement.Gap;
        Take = measurement.Take;
        Success = measurement.Success;
    }
    [XmlElement("measurementID")]
    public int Id { get; set; }
    [XmlElement("orderNr")]
    public int OrderNr { get; set; }
    [XmlElement("pieceNr")]
    public int PieceNr { get; set; }
    [XmlElement("type")]
    public string Type { get; set; }
    [XmlElement("position")]
    public string Position { get; set; }
    [XmlElement("step")]
    public float? Step { get; set; }
    [XmlElement("gap")]
    public float? Gap { get; set; }
    [XmlElement("take")]
    public bool Take { get; set; }
    [XmlElement("success")]
    public string Success { get; set; }    
}
