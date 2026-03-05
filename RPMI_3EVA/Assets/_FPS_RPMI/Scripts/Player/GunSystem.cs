using UnityEngine; 
using UnityEngine.InputSystem;
using System.Collections;

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
    [SerializeField] bool canShoot; //Indica si podemos disparar en X momento del juego
    [SerializeField] bool reloading; //Indica si estamos en proceso de recarga

    #endregion

    private void Awake()
    {
        bulletsLeft = ammoSize; // Al iniciar la partida tenemos el cargador lleno
        canShoot = true; //Al inciar la partida, tenemos la poisibilidad de disparar
    }

  

    // Update is called once per frame
    void Update()
    {
        //Condición estricta de llamar a la corrutina de diaparo
        if (canShoot && shooting && !reloading && bulletsLeft > 0)
        {
            StartCoroutine(ShootRoutine());
        }

    }

    IEnumerator ShootRoutine()
    {
        //La corrutina se va a encargar de medir el tiempo entre diaparos y la gestión del gasto de balas
        //Ademas llamará al raycast de disparo que está definido en Shoot()ç

        canShoot = false; //LLave de seguridadque hace que si estamos disparando no podamos disparar
        if (!allowButtonHold) shooting = false; //Cerrar el bucle de disparo por pulsación
        for(int i = 0;i < bulletsLeft; i++)
        {
            if (bulletsLeft <= 0) break; //Segunda prevención de errores:si no me quedan balas no hago daño
            Shoot(); //Llamada al raycast que define el disparo
            bulletsLeft--; //Resta a la cantidad de balas del cargador actual
        }

        //Espera entre disparos
        yield return new WaitForSeconds(shootingCooldown);
        canShoot = true; //Resetea la posibilidad de disparar
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
            //AQUÍ PUEDO CODEAR TODOS LOS EFECTOS DEL RAYO QUE QUIERO PARA MI INTERACCIÓN
            Debug.Log(hit.collider.name);
        }
    }

    void Reload()
    {
        if (bulletsLeft < ammoSize && !reloading) StartCoroutine(ReloadRoutine());
    }

    IEnumerator ReloadRoutine()
    {
       reloading = true; //Estamos recargando, por lo tanto no podemos recargar
       //AQUÍ LLAMARIAMOS A LA ANIMACIÓN DE RECARGA
       yield return new WaitForSeconds(reloadTime); //Esperar x tiempo que es lo que dura la animación de recarga
        bulletsLeft = ammoSize; //La cantidad de balas actuales se iguala a la máxima
        reloading = false; //termina la recarga, podemos volver a recargar
    }


    #region Input Methods
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (allowButtonHold)
        {
            shooting = context.ReadValueAsButton(); //Detecta constantemente si el boton de disparo está apretado
        }
        else
        {
            if (context.performed) shooting = true; //Shooting solo es true por pulsación
        }
    }
    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.performed) Reload();   
    }

    #endregion



}
