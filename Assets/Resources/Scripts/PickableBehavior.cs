using FMODUnity;
using TMPro;
using UnityEngine;

[ExecuteAlways]
public class PickableBehavior : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private float meshRotationSpeed;
    [SerializeField] private HUDReloadIcon reloadIcon;
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private float rechargeTime;
    private float _currentRechargeTime;

    //TEST VAR
    [SerializeField] private Weapon weapon;

    [SerializeField, Header("SFX"), EventRef]
    private string clockTickSFX;

    [SerializeField, EventRef] private string itemAvailableSFX;

    private Item _item;

    public Item Item
    {
        get => _item;
        set
        {
            mesh.transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);
            
            mesh.enabled = value != null;
            if (value != null)
                mesh.material.SetTexture("_MainTex", value.Texture);

            _item = value;
        }
    }

    private void OnValidate()
    {
        if (mesh == null || mesh.sharedMaterial == null)
            return;

        var tex =
            (weapon == null || weapon.Texture == null)
                ? Resources.Load<Texture2D>("Textures/PickableIcon")
                : weapon.Texture;

        var tempMaterial = new Material(mesh.sharedMaterial);
        tempMaterial.SetTexture("_MainTex", tex);
        mesh.sharedMaterial = tempMaterial;
    }

    void Start()
    {
        mesh.transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);

        if (!Application.isPlaying) return;

        _currentRechargeTime = 0;
        reloadIcon.DisableReloadIcon();
        countDownText.gameObject.SetActive(false);
        Item = new Weapon(weapon);
    }

    void Update()
    {
        if (mesh != null)
            mesh.transform.Rotate(0, meshRotationSpeed * Time.deltaTime, 0);

        if (_currentRechargeTime > 0)
        {
            _currentRechargeTime -= Time.deltaTime;


            if (_currentRechargeTime < 3)
            {
                if (!countDownText.gameObject.activeSelf)
                    countDownText.gameObject.SetActive(true);

                int countDownValue = (int) _currentRechargeTime + 1;
                if (countDownText.text != countDownValue.ToString())
                {
                    countDownText.text = countDownValue.ToString();
                    RuntimeManager.PlayOneShot(clockTickSFX, transform.position);
                }
            }
        }
        else if (_currentRechargeTime < 0)
            OnRecharge();
    }

    private void OnTriggerEnter(Collider other)
    {
        var playerController = other.gameObject.GetComponent<PlayerController>();
        if (playerController == null || Item == null)
            return;

        var weapon = (Weapon) Item;
        if (weapon == null)
            return;

        Item = playerController.Weapon;
        playerController.Weapon = weapon;

        if (Item == null)
        {
            _currentRechargeTime = rechargeTime;
            reloadIcon.EnableReloadIcon(() => _currentRechargeTime, rechargeTime);
        }
    }

    private void OnRecharge()
    {
        _currentRechargeTime = 0;
        reloadIcon.DisableReloadIcon();
        countDownText.gameObject.SetActive(false);

        RuntimeManager.PlayOneShot(itemAvailableSFX, transform.position);

        Item = new Weapon(weapon);
    }
}