﻿using MiniJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class Socket : MonoBehaviour {

	private string interval = "14400";
	private float size = 0.05f;
	
	private string OHLC_ENDPOINT = "https://api.cryptowat.ch/markets/bitflyer/btcfxjpy/ohlc?period=";

	public GameObject candlePrefab;
    
	// Use this for initialization
	void Start () {
		OHLC_ENDPOINT += interval;
		StartCoroutine(generateChart());
	}

	IEnumerator generateChart() {
		yield return GetOHLC();
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
				
				double[][] ohlcDatas = parseOHLCDatas(text);
				instantiateChartObject(ohlcDatas);
 
                // バイナリデータとして取得する
                byte[] results = request.downloadHandler.data;
            } else {
				Debug.Log("Error");
			}
        }
	}

	private double[][] parseOHLCDatas(string responseText) {
		int startArrayIndex = responseText.IndexOf("[[") + 2;
		int endArrayIndex = responseText.IndexOf("]]");

		double[][] ohlcDatas = new double[1000][];
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

	private void instantiateChartObject(double[][] ohlcDatas) {

		double[][] ohlc72 = new double[72][];
		int counter = 71;
		for(int i = ohlcDatas.Length - 1; i >= 0; i-- ) {
			if(ohlcDatas[i] == null) continue;
			ohlc72[counter] = ohlcDatas[i];
			counter--;
			if(counter == -1) break;
		}

		ohlcDatas = ohlc72;

		double highest = 0.0;
		double lowest = 114514191981.0;

		double highestTotal = 0.0;
		double lowestTotal = 114514191981.0;

		int correctCount = 1;
		foreach(double[] ohlc in ohlcDatas) {
			if(ohlc == null) break;
			if(ohlc[2] > highest) highest = ohlc[2];
			if(ohlc[3] < lowest) lowest = ohlc[3];

			if(ohlc[5] > highestTotal) highestTotal = ohlc[5];
			else if(ohlc[5] < lowestTotal) lowestTotal = ohlc[5];
			correctCount++;
		}

		double diff = (highest - lowest) * 0.05;
		highest += diff;
		lowest -= diff;

		GameObject candle = null;

		Vector3 pos = transform.position;

		foreach (double[] ohlc in ohlcDatas) {
			if(ohlc == null) break;
			candle = Instantiate(candlePrefab, pos, transform.rotation);
			candle.name = ohlc[0].ToString();
//			candle.transform.parent = transform;
			candle.transform.SetParent(transform, false);
			//candle.transform.localScale = new Vector3(50, 50, 50);
			CandleStick candleComponent = candle.GetComponent<CandleStick>();
			candleComponent.highest = highest;
			candleComponent.lowest = lowest;
			candleComponent.open = ohlc[1];
			candleComponent.high = ohlc[2];
			candleComponent.low = ohlc[3];
			candleComponent.close = ohlc[4];
			candleComponent.highestTotal = highestTotal;
			candleComponent.lowestTotal = lowestTotal;
			candleComponent.total = ohlc[5];
			pos.x += 0.3f;
		}
		this.transform.localScale = new Vector3(size,size*1.3f,size);
		Vector3 parentPos = this.transform.position;
		parentPos.y -= 1;
		parentPos.x -= (pos.x/2)*size;
		this.transform.position　= parentPos;
	}
}
