using UnityEngine;
using UnityEngine.InputSystem;

public class GunSystem : MonoBehaviour
{

    #region General Variables
    [Header("General References")]
    [SerializeField] Camera fpsCam; //Referencia si disparamos desde el centro de la cam
    [SerializeField] Transform shootPoints; //Ref si queremos disparar desde la punta del cañon
    [SerializeField] LayerMask impactLayer; //Layer con la que el RayCast interactua
    RaycastHit hit; //Almacen de la información de los objetos a los que el raycast puede impactar

    [Header("Weapon parameters")]
    [SerializeField] int damage = 10; //daño del arma por bala
    [SerializeField] float range = 100f; //Distancia del disparo
    [SerializeField] float spread = 0; //Radio de dispersion del arma
    [SerializeField] float shootingCooldown = 0.2f; //Tiempo entre disparos
    [SerializeField] float reloadTime = 1.5f; //Tiempo de recarga
    [SerializeField] bool allowButtonHold = false; //Si el disparo se ejecuta por click(falso) o mantener(true)

    [Header("Bullet Management")]
    [SerializeField] int ammoSize = 30; //Cantidad max de balas por cargador
    [SerializeField] int bulletsPerTap = 1; //Cantidad de balas disparadas por cada ejecucion de disparo
    int bulletsLeft; //Cantidad de balas dentro del cargador actual

    [Header("Feedback References")]
    [SerializeField] GameObject impactEffect; //Referencia al VFX de impacto de la bala

    [Header("Dev - Gun State Bools")]
    [SerializeField] bool shooting; //Indica si estamos disparado
    [SerializeField] bool cansShoot; //Indica si podemos disparar en X momento del juego
    [SerializeField] bool reloading; //Indica si estamos en proceso de recarga

    #endregion

    private void Awake()
    {
        bulletsLeft = ammoSize; // Al iniciar la partida tenemos el cargador lleno
        cansShoot = true; //Al inciar la partida, tenemos la poisibilidad de disparar
    }

  

    // Update is called once per frame
    void Update()
    {
        

    }

    void Shoot()
    {
        //ESTE ES EL ÉTODO MAS IMPORTANTE
        //AQUÍ SE DEFINE EL DISPARO POR RAYCAST = UTILIZABLE CON CUALQUIER MECÁNICA

        //Almacebar la dirección de diapro y modificarla en caso de haber dispersion(spread)
        Vector3 direction = fpsCam.transform.forward; //Se lanza rayo hacia delante de la cámara
        //Añadir dispersión aleatoria segun el valor de dispersion(spread)
        direction.x += Random.Range(-spread, spread);
        direction.y += Random.Range(-spread, spread);

        //DECLARACIÓN DEL RAYCAST
        //Physics.Raycast(Origen del rayo, dirección, almacen de la info del impacto, longitud del rayo, layer con la que impacta el rayo))
        if (Physics.Raycast(fpsCam.transform.position, direction, out hit, range, impactLayer))
        {
            //AQUÍ PUEDO CODEAR TODOS LOS EFECTOS QUE QUIERO PARA MI INTERACIIÓN
            Debug.Log(hit.collider.name);
        }
    }


    #region Input Methods
    public void OnShoot(InputAction.CallbackContext context)
    {

    }
    public void OnReload(InputAction.CallbackContext context)
    {

    }

    #endregion



}
