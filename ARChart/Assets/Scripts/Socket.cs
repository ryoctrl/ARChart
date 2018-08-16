using MiniJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Socket : MonoBehaviour {

	private string OHLC_ENDPOINT = "https://api.cryptowat.ch/markets/bitflyer/btcusd/ohlc";
    
    public Text label;
	// Use this for initialization
	void Start () {
		StartCoroutine(GetOHLC());
		label.text = "";
	}

	IEnumerator GetOHLC() {
		UnityWebRequest request = UnityWebRequest.Get(OHLC_ENDPOINT);

		yield return request.Send();

		if (request.isNetworkError) {
            Debug.Log(request.error);
        } else {
            if (request.responseCode == 200) {
                // UTF8文字列として取得する
                string text = request.downloadHandler.text;
				parseOHLCDatas(text);
				/*
				Dictionary<string, object> json = Json.Deserialize(text) as Dictionary<string, object>;
				json.forEach((key, value) => {
					Debug.Log(key);
					Debug.Log(value);
				});
				Dictionary<string, object> timeJson = (Dictionary<string, object>) json["result"];
				// double[][] timetable = timeJson["14400"] as double[][];
				// Debug.Log(timetable);
				string timetableStr = timeJson["14400"] as string;
				Debug.Log(timetableStr);
				//Dictionary<string, object> timeJson = Json.Deserialize(json["result"])
				//Debug.Log(json["result"]);
				*/
				/*
				Dictionary<string, double[][]> candle = Json.deserialize(text)(Dictionary<string, double[][]>)json["result"];
				double[][] ohlcList = candle["14400"];
				double open = ohlcList[0][0];
				*/
				//Debug.Log (open);
				Debug.Log(text);
 
                // バイナリデータとして取得する
                byte[] results = request.downloadHandler.data;
            } else {
				Debug.Log("Error");
			}
        }
	}

	private double[][] parseOHLCDatas(string responseText) {
		Debug.Log(responseText);
		int startArrayIndex = responseText.IndexOf("[[") + 2;
		int endArrayIndex = responseText.IndexOf("]]");

		double[][] ohlcDatas = new double[500][];
		double[] data = new double[7];
		int dataCount = 0;
		int ohlcCount = 0;
		String num = "";
		for(int i = startArrayIndex; i < endArrayIndex; i++) {
			if(responseText[i] == ']') {
				data[ohlcCount] = double.Parse(num);
				num = "";
				ohlcCount = 0;
				ohlcDatas[dataCount] = data;
				dataCount++;
				i += 2;
				data = new double[7];
			} else if(responseText[i] == ','){
				data[ohlcCount] = double.Parse(num);
				ohlcCount++;
				num = "";
			} else {
				num += responseText[i];
			}
		}
		return ohlcDatas;
	}
}
