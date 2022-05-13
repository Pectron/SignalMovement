using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using System.IO.Compression;

public class WebExporter : MonoBehaviour
{
    public string newParticipantId { get; private set; } = "";
    private bool testMode = false;

    public void SetOnTestMode()
    {
        testMode = true;
        newParticipantId = "Test-Mode";
    }

    public void Send(string answers, string metaDataId, string participant, string dataContentType, string userKeyId, string contentType, bool biosignals)
    {
        byte[] dadosEmByteArray;

        if (!biosignals)
        {
            // Dados da experiência (CSV, etc)
            string dadosDaExperiencia = answers;

            // Converter string em array de bytes
            dadosEmByteArray = System.Text.Encoding.UTF8.GetBytes(dadosDaExperiencia);
        }
        else
        {
            dadosEmByteArray = File.ReadAllBytes(answers);
        }

        // Convert array de bytes em base64
        string dadosEmBase64 = Convert.ToBase64String(dadosEmByteArray);


        // Criar metadados da experiencia
        ExperienceMetaData metaData = new ExperienceMetaData();
        metaData.id = metaDataId;

        // Criar dados da experiencia (inclui metadados)
        ExperienceData data = new ExperienceData();
        data.participants = participant;
        data.data = dadosEmBase64;
        data.dataContentType = dataContentType;
        data.experienceMetaData = metaData;

        // Converter dados da experiencia em JSON
        string strData = JsonUtility.ToJson(data);

        // Mostrar na consola a conversão para JSON
        Debug.Log(strData);

        // Lançar corotina que vai fazer o post
        try
        {
            StartCoroutine(PostData(strData, userKeyId, contentType));
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        
    }

    private IEnumerator PostData(string data, string userKeyId, string contentType)
    {
        using (UnityWebRequest www = UnityWebRequest.Post("https://xperis.herokuapp.com/api/experience-data", data))
        {
            www.SetRequestHeader("user-api-key", userKeyId);

            UploadHandlerRaw newHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(data));
            newHandler.contentType = contentType;
            www.uploadHandler = newHandler;
            www.SetRequestHeader("Content-Type", contentType);

            // Enviar o request, so voltaremos à corotina quando houver um resultado
            yield return www.SendWebRequest();

            // Temos um resultado, mostrar na consola qual
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("POST successful!");
            }
            Debug.Log(www.result);
        }
    }

    public void GetNextId(string metaDataId, string userId)
    {
        try
        {
            StartCoroutine(GetParticipantsID(metaDataId, userId));
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private IEnumerator GetParticipantsID(string metaDataId, string userKeyId)
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://xperis.herokuapp.com/api/experience-meta-data/"+metaDataId))
        {
            www.SetRequestHeader("user-api-key", userKeyId);

            // Enviar o request, so voltaremos à corotina quando houver um resultado
            yield return www.SendWebRequest();

            // Temos um resultado, mostrar na consola qual
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("GET successful!");
                Debug.Log("Data no request: " + www.downloadHandler.text);
                FormatId(www.downloadHandler.text);
            }
            Debug.Log(www.result);


        }
    }

    private void FormatId(string data)
    {
        List<string> participants = new List<string>();
        string[] formatParticipantsAux;

        formatParticipantsAux = data.Split('[');
        Debug.Log("Size of split: " + formatParticipantsAux.Length);
        Debug.Log(formatParticipantsAux[0]);
        formatParticipantsAux = formatParticipantsAux[1].Split(']');
        formatParticipantsAux = formatParticipantsAux[0].Split('}');

        foreach (string s in formatParticipantsAux)
        {
            string[] sArrAux;
            string sAux;

            sAux = s.Trim('{', ',');
            sArrAux = s.Split('"');
            for (int i = 0; i < sArrAux.Length; i++)
            {
                if (sArrAux[i] == "participants")
                {
                    participants.Add(sArrAux[i + 2]);
                    break;
                }
            }
        }

        int highestId = 0;
        string projectName = "";
        foreach (string p in participants.Where(participant => participant != "Test-Mode"))
        {
            string[] idSplit = p.Split('-');
           
            projectName = idSplit[0];
           
            
            if (idSplit.Length > 1 && Int32.Parse(idSplit[1]) > highestId)
            {
                highestId = Int32.Parse(idSplit[1]);
            }
        }
        newParticipantId = $"{projectName}-{++highestId}";
    }
}
