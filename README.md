# MiAppCRUD

Descripción del funcionamiento del login
El sistema de login permite la autenticación de usuarios mediante usuario y contraseña. Utiliza Firebase Authentication para gestionar el registro y acceso a cuentas. Los formularios han sido diseñados para validar datos básicos antes de realizar cualquier operación. Cuando el usuario inicia sesión correctamente, es redirigido a la pantalla principal correspondiente según su categoría. Si la sesión no está activa, el sistema muestra la pantalla de login por defecto.

Proceso de creación de cuenta de usuario
El usuario debe acceder a la pantalla de registro. Una vez allí, se le solicitará un correo electrónico válido, una contraseña que cumpla con los requisitos mínimos de seguridad y la confirmación de la misma. La aplicación valida que ambos campos coincidan y que el correo electrónico tenga el formato adecuado. Luego, se crea la cuenta utilizando Firebase Auth. Las contraseñas nunca se almacenan en texto plano. El proceso de autenticación de Firebase gestiona el hash y cifrado internamente, utilizando algoritmos seguros como bcrypt.

Proceso de inicio de sesión
El usuario accede a la pantalla de login, donde debe ingresar su correo electrónico y contraseña. La aplicación valida que los campos no estén vacíos y que el correo tenga el formato correcto. Si los datos son válidos, se realiza una solicitud a Firebase Auth para autenticar al usuario. Si la combinación es correcta, Firebase emite un token de sesión válido y el usuario es redirigido al menú según su categoría. En caso contrario, se muestran errores personalizados según el tipo de fallo: correo no registrado, contraseña incorrecta o errores de conexión.

Cierre de sesión
El cierre de sesión se realiza de forma manual desde el menú principal, donde se muestra un botón específico para cerrar sesión. Al presionar el botón, se invoca el método signOut() de Firebase Auth, el cual revoca el token de sesión actual. Esto garantiza que el usuario no tendrá acceso a recursos protegidos hasta que vuelva a iniciar sesión. Una vez cerrada la sesión, la aplicación redirige al usuario automáticamente a la pantalla de login.

Validación de datos
Antes de enviar cualquier dato a Firebase, la aplicación implementa validaciones locales en los formularios. Estas validaciones incluyen verificar que los campos no estén vacíos, que el correo electrónico tenga el formato adecuado, que la contraseña cumpla con el mínimo de caracteres requeridos y que las contraseñas coincidan en el formulario de registro. En caso de errores, se muestra un mensaje descriptivo para cada caso. Estas validaciones mejoran la experiencia del usuario y previenen llamadas innecesarias al backend.

Cifrado y seguridad de las contraseñas
La aplicación utiliza Firebase Authentication como sistema de backend seguro. Firebase no almacena contraseñas en texto plano. Internamente, utiliza algoritmos robustos de hashing, como bcrypt, para almacenar las credenciales de forma segura. Al autenticar, la contraseña proporcionada es hasheada y comparada con la versión almacenada. Este enfoque evita la exposición de contraseñas sensibles, incluso en caso de que la base de datos se vea comprometida.

Firebase también implementa mecanismos de seguridad adicionales como protección contra ataques de fuerza bruta, verificación de correo electrónico opcional y límites de intentos de inicio de sesión.

Flujo de navegación y control de sesión
Al iniciar la aplicación, se comprueba si existe un usuario autenticado mediante el método currentUser de Firebase. Si existe una sesión activa, se redirige al usuario a su respectiva pantalla de inicio. Si no existe sesión activa, se muestra la pantalla de login. Esta lógica asegura que los usuarios no puedan acceder a contenido protegido sin estar autenticados.

Este enfoque también permite mantener la sesión abierta mientras el usuario no cierre sesión manualmente, respetando así el flujo natural de navegación en aplicaciones móviles modernas.

Recomendaciones
Se recomienda que los usuarios elijan contraseñas seguras y únicas para evitar vulnerabilidades. También se sugiere implementar la verificación por correo electrónico o autenticación multifactor en futuras versiones para aumentar aún más la seguridad del sistema.
