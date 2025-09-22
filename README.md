# 🚀 API de Autenticación JWT

¡Bienvenido al proyecto de **Autenticación JWT**!  
Esta API es el ejercicio 2 de prueba técnica de Roshka **JSON Web Tokens (JWT)** y controlar el acceso según roles de usuario.  

---

## ✨ Características Principales

- 🔑 **Autenticación Segura:** Validación de identidad de usuarios mediante tokens JWT.
- 🛂 **Autorización por Roles:** Permite/deniega acceso a recursos según el rol (`Admin` / `User`).
- ⚠️ **Gestión de Errores:** Respuestas claras para `401 Unauthorized` y `403 Forbidden`.
- 🧪 **Usuarios en Memoria:** Base de datos simulada para pruebas rápidas y sencillas.
- 📜 **Swagger UI:** Interfaz integrada para probar endpoints desde el navegador.

---

## 📋 Requisitos Previos

Para ejecutar esta aplicación, asegúrate de tener instalado:

- [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download)
- Un cliente REST como **Postman**, **Insomnia** o simplemente usar **Swagger UI**.

---

## ▶️ Primeros Pasos

1. **Clonar el repositorio**

```bash
git clone https://github.com/Bantrax947/https://github.com/Bantrax947/api_roshka_token.git


    Ejecutar la API

dotnet run

La API se levantará en un puerto local. Verás la URL en la consola (por ejemplo:
https://localhost:7150).

    Abrir Swagger (opcional)
    En tu navegador accede a:

https://localhost:7150/swagger

🛡️ Endpoints Disponibles
Método	Endpoint	Descripción
POST	/Auth/login	Genera un token JWT válido con las credenciales de prueba.
GET	/Auth/profile	Devuelve el perfil del usuario autenticado (requiere token).
GET	/Auth/admin-data	Devuelve datos restringidos. Solo para usuarios con rol Admin.
👤 Usuarios de Prueba

Utiliza estas credenciales para autenticarte y generar tokens:
Usuario	Password	Rol
Bantrax	123     	Admin
Franco	123     	User

Ejemplo de flujo de prueba:

    Haz un POST a /Auth/login con el body:

{
  "username": "Bantrax",
  "password": "123"
}

    Copia el token de la respuesta.

    En los siguientes endpoints (/Auth/profile, /Auth/admin-data), agrega el header:

Authorization: Bearer <tu_token>


💡 Notas

    No requiere base de datos real (usuarios en memoria)

    El token tiene expiración de 30 min para mayor seguridad

    Puedes modificar los usuarios de prueba en el código para simular otros escenarios
