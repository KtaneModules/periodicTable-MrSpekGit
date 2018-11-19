	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
	using System.Linq;
	using UnityEngine;
	using Newtonsoft.Json;
	using KMHelper;

	public class ElementsOption
	{
	public String ele;
	public String sym;
	public Int32 num;
	}

	public class ElementsOptions
	{
	public List<ElementsOption> options;
	}

	public class periodicTableScript : MonoBehaviour 
	{
	public TextAsset elementsJson;
	public KMBombInfo Bomb;
	public KMAudio Audio;
	public TextMesh eleScreen, symScreen, numScreen;
	public Color[] eleColour, symColour, numColour;
	public Material[] butColour;
	public Renderer[] buttonColour;
	public KMSelectable[] buttons;


	ElementsOptions eleOptions, symOptions, numOptions, butOptions, pressOptions, answerOptions, twitchOptions;
	ElementsOption eleOption, symOption, numOption, butOption, pressOption, answerOption, twitchOption;

	//logging
	static int moduleIdCounter = 1;
	int moduleId;
	private bool moduleSolved;

	//bools
	bool isSolved;

	//strings
	string elementColourName;
	string symbolColourName;
	string numberColourName;

	//Ints
	int randomButton;
	int randomButtonColour;
	int randomElementColour;
	int randomSymbolColour;
	int randomNumberColour;
	int solutionButton;
	int solutionButtonMod;
	int solutionButtonModII;
	int timesModded;
	int butPrs;

	void Awake()
	{
		moduleId = moduleIdCounter++;

		for (int i = 0; i < buttons.Length; i++) {
			int j = i;
			buttons [j].OnInteract += delegate () {
				buttonInteract (j);
				return false;
			};
		}
	}

	void Start () 
	{
		ChooseElement ();
		ChooseSymbol ();
		ChooseNumber ();
		ChooseButtonColour ();
		CalculateAnswers ();
		CalculateAnswer ();
		string solutionButtonModString = solutionButtonMod.ToString ();
		string timesModdedString = timesModded.ToString ();
		Debug.LogFormat ("[Periodic Table #{0}] (Ele + Sym + Num + But) - ({1} * 118) = {2}", moduleId, timesModdedString, solutionButtonModString);
	}

	//Choose Element on screen
	void ChooseElement()
	{
		eleOptions = JsonConvert.DeserializeObject<ElementsOptions>(elementsJson.text);
		eleOption = eleOptions.options[UnityEngine.Random.Range(0, eleOptions.options.Count-4)];
		randomElementColour = randomElementColour + UnityEngine.Random.Range(0, 6);
		eleColToName ();
		string eleString = eleOption.num.ToString ();
		Debug.LogFormat("[Periodic Table #{0}] The chosen Element is {1} / {2} / {3} and the colour is {4}", moduleId, eleOption.ele, eleOption.sym, eleString, elementColourName);
		eleScreen.text = eleOption.ele;
		eleScreen.color = eleColour [randomElementColour];
	}

	//Turning random colour into string name
	void eleColToName()
	{
		if (randomElementColour == 0) {
			elementColourName = "Red";
		}
		else if (randomElementColour == 1) {
			elementColourName = "Orange";
		}
		else if (randomElementColour == 2) {
			elementColourName = "Yellow";
		}
		else if (randomElementColour == 3) {
			elementColourName = "Green";
		}
		else if (randomElementColour == 4) {
			elementColourName = "Blue";
		}
		else {
			elementColourName = "White";
		}
	}

	//Choose Symbol on screen
	void ChooseSymbol()
	{
		symOptions = JsonConvert.DeserializeObject<ElementsOptions>(elementsJson.text);
		symOption = symOptions.options[UnityEngine.Random.Range(0, symOptions.options.Count-4)];
		randomSymbolColour = randomSymbolColour + UnityEngine.Random.Range(0, 6);
		symColToName ();
		string symString = symOption.num.ToString ();
		Debug.LogFormat("[Periodic Table #{0}] The chosen Symbol is {1} / {2} / {3} and the colour is {4}", moduleId, symOption.ele, symOption.sym, symString, symbolColourName);
		symScreen.text = symOption.sym;
		symScreen.color = symColour [randomSymbolColour];
	}

	//Turning random colour into string name
	void symColToName()
	{
		if (randomSymbolColour == 0) {
			symbolColourName = "Red";
		}
		else if (randomSymbolColour == 1) {
			symbolColourName = "Orange";
		}
		else if (randomSymbolColour == 2) {
			symbolColourName = "Yellow";
		}
		else if (randomSymbolColour == 3) {
			symbolColourName = "Green";
		}
		else if (randomSymbolColour == 4) {
			symbolColourName = "Blue";
		}
		else {
			symbolColourName = "White";
		}
	}

	//Choose Number on screen
	void ChooseNumber()
	{
		numOptions = JsonConvert.DeserializeObject<ElementsOptions>(elementsJson.text);
		numOption = numOptions.options[UnityEngine.Random.Range(0, numOptions.options.Count-4)];
		randomNumberColour = randomNumberColour + UnityEngine.Random.Range(0, 6);
		numColToName ();
		string numString = numOption.num.ToString ();
		Debug.LogFormat("[Periodic Table #{0}] The chosen Number is {1} / {2} / {3} and the colour is {4}", moduleId, numOption.ele, numOption.sym, numString, numberColourName);
		numScreen.text = numString;
		numScreen.color = numColour [randomNumberColour];
	}

	//Turning random colour into string name
	void numColToName()
	{
		if (randomNumberColour == 0) {
			numberColourName = "Red";
		}
		else if (randomNumberColour == 1) {
			numberColourName = "Orange";
		}
		else if (randomNumberColour == 2) {
			numberColourName = "Yellow";
		}
		else if (randomNumberColour == 3) {
			numberColourName = "Green";
		}
		else if (randomNumberColour == 4) {
			numberColourName = "Blue";
		}
		else {
			numberColourName = "White";
		}
	}

	//Choose Button to colour
	void ChooseButtonColour()
	{
		randomButton = randomButton + UnityEngine.Random.Range(0, 118);
		randomButtonColour = randomButtonColour + UnityEngine.Random.Range(0, 6);
		int realRandomButton = randomButton + 1;
		butOptions = JsonConvert.DeserializeObject<ElementsOptions>(elementsJson.text);
		butOption = butOptions.options [randomButton];
		string realRandomButtonString = realRandomButton.ToString ();
		Debug.LogFormat ("[Periodic Table #{0}] The chosen Button is {1} / {2} / {3} and the colour is {4}", moduleId, butOption.ele, butOption.sym, realRandomButtonString, butColour[randomButtonColour].name);
		buttonColour [randomButton].material = butColour[randomButtonColour];
	}

	void CalculateAnswers()
	{

		Debug.LogFormat ("[Periodic Table #{0}] ---------------- ", moduleId);
		//Element# + Batteries
		int batCount = Bomb.GetBatteryCount();
		randomElementColour = randomElementColour + 1;
		int eleAnswer = (eleOption.num + batCount)*(randomElementColour);
		string eleString = eleOption.num.ToString ();
		string batCountString = batCount.ToString ();
		string eleAnswerString = eleAnswer.ToString ();
		string randomElementColourString = randomElementColour.ToString ();
		Debug.LogFormat ("[Periodic Table #{0}] ( Element ({1}) + Batteries ({2}) ) * {3}({4}) = {5}", moduleId, eleString, batCountString, elementColourName, randomElementColourString, eleAnswerString);

		//Symbol# + Ports
		int portCount = Bomb.GetPortCount();
		randomSymbolColour = randomSymbolColour + 1;
		int symAnswer = (symOption.num + portCount)*(randomSymbolColour);
		string symString = symOption.num.ToString ();
		string portCountString = portCount.ToString ();
		string symAnswerString = symAnswer.ToString ();
		string randomSymbolColourString = randomSymbolColour.ToString ();
		Debug.LogFormat ("[Periodic Table #{0}] ( Symbol ({1}) + Ports ({2}) ) * {3}({4}) = {5}", moduleId, symString, portCountString, symbolColourName, randomSymbolColourString, symAnswerString);

		//Number# + Indicators
		int indiCount = Bomb.GetOnIndicators().Count() + Bomb.GetOffIndicators().Count();
		randomNumberColour = randomNumberColour + 1;
		int numAnswer = (numOption.num + indiCount)*(randomNumberColour);
		string numString = numOption.num.ToString ();
		string indiCountString = indiCount.ToString ();
		string numAnswerString = numAnswer.ToString ();
		string randomNumberColourString = randomNumberColour.ToString ();
		Debug.LogFormat ("[Periodic Table #{0}] ( Number ({1}) + Indicators ({2}) ) * {3}({4}) = {5}", moduleId, numString, indiCountString, numberColourName, randomNumberColourString, numAnswerString);

		//Button# + SNDigits
		int SNDigitsSum = Bomb.GetSerialNumberNumbers().Sum();
		randomButtonColour = randomButtonColour + 1;
		int butAnswer = (butOption.num + SNDigitsSum)*(randomButtonColour);
		string butString = butOption.num.ToString ();
		string SNDigitsSumString = SNDigitsSum.ToString ();
		string butAnswerString = butAnswer.ToString ();
		string randomButtonColourString = randomButtonColour.ToString ();
		Debug.LogFormat ("[Periodic Table #{0}] ( Button ({1}) + Sum Digits Serial ({2}) ) * {3}({4}) = {5}", moduleId, butString, SNDigitsSumString, butColour[randomButtonColour-1].name, randomButtonColourString, butAnswerString);

		//Calculate solution
		solutionButton = eleAnswer + symAnswer + numAnswer + butAnswer;
		string solutionButtonString = solutionButton.ToString ();
		Debug.LogFormat ("[Periodic Table #{0}] Ele + Sym + Num + But = {1}", moduleId, solutionButtonString);
	}

	 void CalculateAnswer()
	{
		if (solutionButton > 118) {
			solutionButton = solutionButton - 118;
			timesModded = timesModded + 1;
			CalculateAnswer ();
		} 

		else {
			solutionButtonMod = solutionButton;
			solutionButtonModII = solutionButton - 1;
		}
	}

	void buttonInteract(int i)
	{
		{ 
			buttons[i].AddInteractionPunch();
			pressOptions = JsonConvert.DeserializeObject<ElementsOptions>(elementsJson.text);
			pressOption = pressOptions.options[i];
			Debug.LogFormat ("[Periodic Table #{0}] ---------------- ", moduleId);
			Debug.LogFormat ("[Periodic Table #{0}] You pressed {1} / {2} / {3}", moduleId, pressOption.ele, pressOption.sym, pressOption.num);

			if (isSolved)
			{
				return;
			}

			else if (i.Equals(solutionButtonModII)){
				answerOptions = JsonConvert.DeserializeObject<ElementsOptions>(elementsJson.text);
				answerOption = answerOptions.options[solutionButtonModII];
				Debug.LogFormat ("[Periodic Table #{0}] Expecting {1} / {2} / {3}", moduleId, answerOption.ele, answerOption.sym, answerOption.num);
				Debug.LogFormat ("[Periodic Table #{0}] You solved the module :D", moduleId);
				Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
				isSolved = true;
				GetComponent<KMBombModule>().HandlePass();
			}

			else {
				answerOptions = JsonConvert.DeserializeObject<ElementsOptions>(elementsJson.text);
				answerOption = answerOptions.options[solutionButtonModII];
				Debug.LogFormat ("[Periodic Table #{0}] Expecting {1} / {2} / {3}", moduleId, answerOption.ele, answerOption.sym, answerOption.num);
				Debug.LogFormat ("[Periodic Table #{0}] That was wrong :(", moduleId);
				GetComponent<KMBombModule>().HandleStrike();
			}

			return;
		}
	}

	#pragma warning disable 414
	private string TwitchHelpMessage = @"Submit the atomic number as an answer with “!{0} submit 109” or “!{0} press 109”. This ssubmits atomic number 109 (Must be digits!).";
	#pragma warning restore 414

	private IEnumerator ProcessTwitchCommand(string inputCommand)
	{
		var commands = inputCommand.ToLowerInvariant().Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
		int final;

		if (commands.Length != 2 || (commands [0] != "submit" && commands [0] != "press")) 
		{
			yield break;
		}

		string result = commands [1];

		if (Int32.TryParse(result, out final)) 
		{
			if (!(final > buttons.Length) && !(final == 0)) 
			{
				yield return null;
				int finalII = final - 1;
				buttons [finalII].OnInteract ();
				yield return new WaitForSeconds(.1f);
			} 

			else 
			{
				yield break;
			}
		}

		else 
		{
			yield break;
		}
	}
}