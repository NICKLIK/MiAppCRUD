# Implementacion de Patrones de Diseno y Principios SOLID al Core MVC
Proyecto desarrollado en **ASP.NET Core** + **PostgreSQL** + **React** (Vite) como frontend.


Inicialmente el proyecto fue desarrollado de manera funcional pero con bajo nivel de desacoplamiento. Posteriormente, se realizó una refactorización para aplicar buenas prácticas como **principios SOLID** y **patrones de diseño**, logrando una arquitectura mucho más escalable y mantenible.


Estado Inicial del Proyecto (Antes de la Refactorización)

Acceso directo a **DbContext** desde los Services.
Lógica de negocio y persistencia de datos totalmente mezclada.
Sin separación de responsabilidades claras.
Sin uso de **interfaces**, **repositorios** o **factories**.
Controllers dependientes de **clases concretas**, no de abstracciones.


Principios SOLID Aplicados
S - Single Responsibility Principle (SRP): Cada clase tiene una única responsabilidad (Repositorios solo acceden a datos, Services solo ejecutan lógica de negocio, Factories solo crean objetos) 
D - Dependency Inversion Principle (DIP): Los servicios y controladores dependen de **interfaces** (abstracciones), no de clases concretas. Inyección de dependencias implementada 


Patrones de Diseño Aplicados

Repository Pattern: Se crearon interfaces y clases concretas por cada módulo: `IProductoRepository`, `IUsuarioRepository`, etc. Separando por completo la lógica de persistencia 
Factory Pattern:  Se implementaron factories para la creación de entidades con lógica especial o inicialización personalizada 


Cambios Estructurales Realizados

Servicios accedían directo a DbContext | Servicios ahora solo llaman a Repositorios 
Creación de entidades hecha a mano dentro de los servicios | Uso de Factories para centralizar la creación 
Alta dependencia de Entity Framework | Lógica de negocio totalmente desacoplada de la capa de acceso a datos 
Sin interfaces | Todas las dependencias ahora están abstraídas por interfaces 


Módulos refactorizados

Productos: SRP + DIP + Repository + Factory 
Usuarios: SRP + DIP + Repository + Factory 
Categorías: SRP + DIP + Repository 
Reabastecimiento de Stock: SRP + DIP + Repository + Factory 

Beneficios Técnicos Obtenidos

Código modular, mantenible y fácil de testear.
Cumplimiento de estándares profesionales de arquitectura.
Facilidad para cambiar motor de base de datos o capa de persistencia.
Preparación para futuras extensiones o migración a microservicios.


# AVANCE, PRESENTACIÓN Y DEFENSA DEL CORE MVC - ECUNISHOP - TIENDA VIRTUAL DE PRODUCTOS


Es una aplicación fullstack desarrollada con React (Vite) en el frontend y ASP.NET Core + PostgreSQL en el backend. Su propósito es administrar un catálogo de productos, gestionar eventos promocionales, controlar el stock y permitir la experiencia de compra para usuarios, todo dentro de un entorno moderno, dinámico y escalable.

---

Tecnologías Utilizadas

Frontend
- React (Vite)
- CSS personalizado
- React Router DOM
- LocalStorage para gestión de carrito de compras

Backend
-  ASP.NET Core 6
-  PostgreSQL en Render
-  Entity Framework Core
-  Roles, validaciones y autenticación

---

Funcionalidades Generales

Panel Administrador
- CRUD de Categorías de Producto
- CRUD de Productos (con imagen, precio, ecunipoints, stock)
- Peticiones de Reabastecimiento (fecha programada, cantidad, estado)
- Gestión de Eventos Promocionales
- CRUD de Usuarios (con rol admin o usuario normal)

 Módulo de Catálogo (Usuario Final)
- Visualización de productos por categoría
- Detalles de cada producto (modal con botones)
- Sistema de carrito de compras
- Reserva de productos en localStorage
- Visualización de eventos activos destacados

---

Funcionalidades del Core

Análisis y Gestión del Stock
- Validación automática de productos con stock > 50 para incluirlos en eventos
- Actualización de stock solo a través del sistema de reabastecimiento programado

Gestión de Eventos
- Creación de eventos promocionales por:
  - Categorías (todos los productos de una categoría con stock suficiente)
  - Productos individuales
- Aplicación de descuentos por porcentaje
- Visualización de eventos registrados
- Eliminación y validación de eventos activos
- Visualización de productos destacados en el catálogo cuando hay eventos activos

Reportes Inteligentes (Módulo separado)
- Ingresos por producto, categoría y ubicación
- Recomendaciones automáticas para futuras promociones

---

Estructura General del Proyecto

MiAppCRUD.Server/
├── Controllers/
│ └── EventoController.cs
├── Models/
│ ├── Producto.cs
│ ├── CategoriaProducto.cs
│ ├── Evento.cs
│ ├── EventoDto.cs
│ ├── ProductoDto.cs
│ └── CategoriaProductoDto.cs
├── Services/
│ ├── EventoService.cs
│ └── EventoServiceImpl.cs
└── Data/
└── AppDbContext.cs

miappcrud.client/
├── views/
│ ├── Catalogo.jsx
│ ├── CarritoCompras.jsx
│ ├── GestionCatalogo.jsx
│ ├── GestionEventos.jsx
│ └── ProductosAdmin.jsx
└── assets/
└── Productos.css

---

Estado del Proyecto

- Sistema de autenticación con roles (admin y usuario)
- CRUD completo de productos y categorías
- Gestión de stock programada
- Sistema de eventos funcional
- Frontend y backend integrados
- Base de datos desplegada en Render


# DEBER ADMIN MVC
Validación de Correo Electrónico en Backend
Validación de Unicidad de Correo Electrónico
Propósito
Garantizar que cada dirección de correo electrónico sea única en el sistema, previniendo:
Creación de múltiples cuentas con el mismo correo
Conflictos en la identificación de usuarios
Problemas en procesos de recuperación de contraseña

Consideraciones de Seguridad
No revelar información: El mensaje de error es genérico ("Credenciales inválidas") en login
Hash seguro: El correo se almacena tal cual para funcionalidades de negocio
Índice único: Recomendable en campo Correo en la base de datos

Flujo de Validación Completo
Frontend (validación inicial):
Verifica formato con regex
llama a endpoint /validar-correo mientras el usuario escribe
Backend (validación definitiva):
Verifica en base de datos antes de crear/actualizar
Retorna error 400 con mensaje claro si el correo existe

Validación de Correo Electrónico
Implementación:
Se agregó validación de formato con regex ^[^\s@]+@[^\s@]+\.[^\s@]+$
Validación de unicidad en backend antes de registrar
Mensajes de error específicos:
"El correo electrónico no es válido" (formato incorrecto)
"El correo electrónico ya está registrado" (duplicado)

Validaciones Implementadas

Validación de Consistencia:
No se puede seleccionar ciudad sin provincia
La ciudad debe pertenecer a la provincia seleccionada
Mensaje de error: "La ciudad no pertenece a la provincia seleccionada"

Flujo de Selección:
Dropdown de ciudades se habilita solo después de seleccionar provincia
Las ciudades se cargan dinámicamente según provincia seleccionada


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


