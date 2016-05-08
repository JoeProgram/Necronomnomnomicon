using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour {

	public SpriteRenderer title;
	public SpriteRenderer textBox;
	public SpriteRenderer keyZ;
	public SpriteRenderer whiteBox;
	public SpriteRenderer cth;

	public AudioClip sfxTextBox;
	public AudioClip sfxSpell;

	public Camera zoomInCam;

	public ParticleSystem pfxReveal;

	protected bool next = false;
	public GUIStyle style;



	protected string textBoxSpeech;



	void Awake(){
		textBox.transform.localScale = Vector3.zero;
	}

	void Start(){
		StartCoroutine (Plot());
	}

	protected IEnumerator Plot(){

		while (!next) yield return null;
		next = false;

		iTween.FadeTo(title.gameObject, iTween.Hash("time",2,"alpha",0.5f,"easetype","linear"));

		yield return new WaitForSeconds (1.0f);

		//bring up the text box
		iTween.ScaleTo(textBox.gameObject,Vector3.one,1.0f);
		yield return new WaitForSeconds(1.0f);

		yield return new WaitForSeconds (0.5f);
		textBoxSpeech = "Is this safe?";
		keyZ.enabled = true;

		// wait for player input
		while (!next) yield return null;
		AudioSource.PlayClipAtPoint (sfxTextBox, Vector3.zero);
		next = false;

		textBox.transform.localScale = new Vector3 (-textBox.transform.localScale.x, 1, 1);
		iTween.PunchScale (textBox.gameObject, Vector3.one * 0.3f, 0.5f);
		textBoxSpeech = "No, but we must summon him to our plane to defeat him.";

		while (!next) yield return null;
		AudioSource.PlayClipAtPoint (sfxTextBox, Vector3.zero);
		next = false;

		textBox.transform.localScale = new Vector3 (-textBox.transform.localScale.x, 1, 1);
		iTween.PunchScale (textBox.gameObject, Vector3.one * 0.3f, 0.5f);
		textBoxSpeech = "The arcane symbol is drawn.\nThe candles have been lit.\nAll that remains is to utter the forbidden words of the unholy book.";

		while (!next) yield return null;
		AudioSource.PlayClipAtPoint (sfxTextBox, Vector3.zero);
		next = false;

		textBox.transform.localScale = new Vector3 (-textBox.transform.localScale.x, 1, 1);
		iTween.PunchScale (textBox.gameObject, Vector3.one * 0.3f, 0.5f);
		textBoxSpeech = "From that starless universe, devoid of hope, knowing only ravishing hunger...";
		
		while (!next) yield return null;
		AudioSource.PlayClipAtPoint (sfxTextBox, Vector3.zero);
		next = false;

		textBox.transform.localScale = new Vector3 (-textBox.transform.localScale.x, 1, 1);
		iTween.PunchScale (textBox.gameObject, Vector3.one * 0.3f, 0.5f);
		textBoxSpeech = "CTH'XYZ'TSGRG, WE SUMMON THEE!";
		
		while (!next) yield return null;
		AudioSource.PlayClipAtPoint (sfxTextBox, Vector3.zero);
		next = false;

		textBoxSpeech = "";
		keyZ.GetComponent<Renderer>().enabled = false;
		whiteBox.GetComponent<Renderer>().enabled = true;
		textBox.transform.localScale = Vector3.zero;
		cth.GetComponent<Renderer>().enabled = true;
		cth.transform.localScale = new Vector3 (0.3f, 0.3f, 1);
		iTween.FadeTo (whiteBox.gameObject, 0, 3f);
		AudioSource.PlayClipAtPoint (sfxSpell, Vector3.zero);
		yield return new WaitForSeconds (0.5f);

		pfxReveal.Play ();
		yield return new WaitForSeconds (2.5f);

		iTween.ValueTo (gameObject, iTween.Hash ("from", Camera.main.orthographicSize, "to", zoomInCam.orthographicSize, "onupdate", "UpdateCam1","easetype",iTween.EaseType.easeInOutCubic,"time",2.0f));
		iTween.MoveTo (Camera.main.gameObject, iTween.Hash ("position", zoomInCam.transform.position, "easetype",iTween.EaseType.easeInOutCubic,"time",2.0f));

		yield return new WaitForSeconds (1.0f);

		//bring up the text box
		iTween.ScaleTo(textBox.gameObject,Vector3.one,1.0f);
		yield return new WaitForSeconds(1.0f);
		
		yield return new WaitForSeconds (0.5f);
		textBoxSpeech = "What, is that him?";
		keyZ.enabled = true;

		while (!next) yield return null;
		AudioSource.PlayClipAtPoint (sfxTextBox, Vector3.zero);
		next = false;

		textBox.transform.localScale = new Vector3 (-textBox.transform.localScale.x, 1, 1);
		iTween.PunchScale (textBox.gameObject, Vector3.one * 0.3f, 0.5f);
		textBoxSpeech = "Couldn't be.\nIt's only got one all-seeing eye,\none gnashing maw, one grasping tentacle.";

		while (!next) yield return null;
		AudioSource.PlayClipAtPoint (sfxTextBox, Vector3.zero);
		next = false;

		textBox.transform.localScale = new Vector3 (-textBox.transform.localScale.x, 1, 1);
		iTween.PunchScale (textBox.gameObject, Vector3.one * 0.3f, 0.5f);
		textBoxSpeech = "Maybe we did it wrong?";

		while (!next) yield return null;
		AudioSource.PlayClipAtPoint (sfxTextBox, Vector3.zero);
		next = false;

		textBox.transform.localScale = new Vector3 (-textBox.transform.localScale.x, 1, 1);
		iTween.PunchScale (textBox.gameObject, Vector3.one * 0.3f, 0.5f);
		textBoxSpeech = "I knew you would mess up the eldritch symbol.\nLeave it, we'll try again tomorrow.";

		while (!next) yield return null;
		AudioSource.PlayClipAtPoint (sfxTextBox, Vector3.zero);
		next = false;

		iTween.CameraTexture (Color.black);
		iTween.CameraFadeTo (1.0f, 2.0f);
		yield return new WaitForSeconds (2.0f);

		Application.LoadLevel(Application.loadedLevel + 1);



	}



	// Update is called once per frame
	void Update () {

		if (Input.anyKeyDown) {
			next = true;
		}
	}

	void OnGUI(){
		GUI.Label (new Rect (0, 20, Screen.width, 170), textBoxSpeech, style);
	}

	protected void UpdateCam1(float size){
		Camera.main.orthographicSize = size;
	}
}
