using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class InspectionPlanReader : MonoBehaviour
{
    [SerializeField] mqttReceiver mqttReceiver;
    public InspectionPlanData MqttData { get; set; }
    public delegate void InspectionPlanReceived(string inspectionPlanName);
    public static event InspectionPlanReceived OnInspectionPlanReceived;

    void OnEnable()
    {
        mqttReceiver.OnMessageArrived += LoadXMLPlan;
    }
    void OnDisable()
    {
        mqttReceiver.OnMessageArrived -= LoadXMLPlan;
    }
    public void LoadXMLPlan(string xml)
    {
        Debug.Log("MQTT OnMessageArrived event fired. The message is = " + xml);
        XmlSerializer serializer = new XmlSerializer(typeof(InspectionPlanData));
        InspectionPlanData mqttData;

        using (TextReader reader = new StringReader(xml))
        {
            mqttData = (InspectionPlanData)serializer.Deserialize(reader);
        }
        Debug.Log("MQTT: New XML Inspection Plan deserialized.");
        //Debug.Log("Deserialized. Test2: " + mqttData.AllMeasurements.Measurements[0].Type);
        //TextWriter writer = new StreamWriter("C:/Users/admin/Desktop/protocol3.xml");
        //serializer.Serialize(writer, mqttData);
        //writer.Close();
        MqttData = mqttData;
        if (OnInspectionPlanReceived != null)
            OnInspectionPlanReceived(mqttData.InspectionPlan.Name);

    }

    /// <summary>
    /// Just for testing
    /// </summary>
    public void LoadXML()
    {
        GetComponent<TextMeshPro>().text = "testi";
        XmlSerializer serializer = new XmlSerializer(typeof(InspectionPlanData));
        FileStream fs = new FileStream("C:/Users/admin/Desktop/protocol.xml", FileMode.Open);
        InspectionPlanData data;
        TextWriter writer = new StreamWriter("C:/Users/admin/Desktop/protocol2.xml");
        //Data data2 = new Data();
        //data2.InspectionPlan = new InspectionPlan();
        //data2.InspectionPlan.Id = 2;
        //data2.InspectionPlan.Name = "Oles Plan";
        //data2.InspectionPlan.Success = "true";
        //data2.AllInspections = new AllInspections();
        //data2.AllInspections.Inspections = new List<Inspection>();
        //Inspection ersterInspect = new Inspection();
        //ersterInspect.Id = 3;
        //ersterInspect.InspectionPlanId = 2;
        //ersterInspect.Success = "true";
        //ersterInspect.Name = "una inspect";
        //Inspection zweiterInspect = ersterInspect;
        //zweiterInspect.Name = "zweii";
        //data2.AllInspections.Inspections.Add(ersterInspect);
        //data2.AllInspections.Inspections.Add(zweiterInspect);

        data = (InspectionPlanData)serializer.Deserialize(fs);
        string tempi = data.InspectionPlan.Name;
        Debug.Log("Deserialized. Test: " + tempi);
        Debug.Log("Deserialized. Test2: " + data.AllMeasurements.Measurements[0].Type);

        serializer.Serialize(writer, data);
        writer.Close();

    }

}

[XmlRoot("data")]
public class InspectionPlanData
{
    [XmlElement(ElementName = "inspectionPlan")]
    public InspectionPlan InspectionPlan { get; set; }

    [XmlElement(ElementName = "allInspections")]
    public AllInspections AllInspections { get; set; }

    [XmlElement(ElementName = "allMeasurements")]
    public AllMeasurements AllMeasurements { get; set; }

}

[XmlRoot("allInspections")]
public class AllInspections
{
    [XmlElement(ElementName = "inspection")]
    public List<Inspection> Inspections { get; set; }
}

[XmlRoot("allMeasurements")]
public class AllMeasurements
{
    [XmlElement(ElementName = "measurement")]
    public List<Measurement> Measurements { get; set; }
}

[XmlRoot("inspectionPlan")]
public class InspectionPlan
{
    [XmlElement("id")]
    public int Id { get; set; }
    [XmlElement("name")]
    public string Name { get; set; }
    [XmlElement("success")]
    public string Success { get; set; }
}

[XmlRoot("measurement")]
public class Measurement
{
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
    [XmlElement("stepMin")]
    public float StepMin { get; set; }
    [XmlElement("stepMax")]
    public float StepMax { get; set; }
    [XmlElement("gapMin")]
    public float GapMin { get; set; }
    [XmlElement("gapMax")]
    public float GapMax { get; set; }
    [XmlElement("step")]
    public string step
    {
        get
        {
            if (this.Step.HasValue)
                return this.Step.Value.ToString();
            else
                return null;
        }
        set
        {
            if (!string.IsNullOrEmpty(value))
                this.Step = float.Parse(value, CultureInfo.InvariantCulture);
            else
                this.Step = null;
        }
    }
    [XmlIgnore]
    public float? Step { get; set; }
    [XmlElement("gap")]
    public string gap
    {
        get
        {
            if (this.Gap.HasValue)
                return this.Gap.Value.ToString();
            else
                return null;
        }
        set
        {
            if (!string.IsNullOrEmpty(value))
                this.Gap = float.Parse(value, CultureInfo.InvariantCulture);
            else
                this.Gap = null;
        }
    }
    [XmlIgnore]
    public float? Gap { get; set; }
    [XmlElement("take")]
    public bool Take { get; set; }
    [XmlElement("success")]
    public string Success { get; set; }
    [XmlElement("inspectionID")]
    public int InspectionId { get; set; }
}

[XmlRoot("inspection")]
public class Inspection
{
    [XmlElement("id")]
    public int Id { get; set; }
    [XmlElement("name")]
    public string Name { get; set; }
    [XmlElement("success")]
    public string Success { get; set; }
    [XmlElement("inspectionPlanID")]
    public int InspectionPlanId { get; set; }
}
/*
[XmlRoot("measurements")]
public class AllMeasurementResults
{
    public List<MeasurementResult> MeasurementResults { get; set; }
}


[XmlRoot("measurement")]
public class MeasurementResult
{
    [XmlElement("id")]
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
    public float Step { get; set; }
    [XmlElement("gap")]
    public float Gap { get; set; }
    [XmlElement("take")]
    public string Take { get; set; }
    [XmlElement("success")]
    public string Success { get; set; }
    [XmlElement("inspectionID")]
    public int InspectionId { get; set; }
}
*/