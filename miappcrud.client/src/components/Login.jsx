import { useState } from "react";
import { useNavigate } from "react-router-dom";
import CryptoJS from "crypto-js";

function Login({ setEstaLogueado }) {
    const [nombreUsuario, setNombreUsuario] = useState("");
    const [contrasena, setContrasena] = useState("");
    const navigate = useNavigate();

    const encriptarMD5 = (str) => {
        return CryptoJS.MD5(str).toString();
    };

    const validarContrasena = (pass) => {
        const tieneLongitud = pass.length >= 8;
        const tieneMayuscula = /[A-Z]/.test(pass);
        const tieneSimbolo = /[.\-_/@]/.test(pass);
        return tieneLongitud && tieneMayuscula && tieneSimbolo;
    };

    const iniciarSesion = async () => {
        if (!validarContrasena(contrasena)) {
            alert("La contrasena debe tener al menos 8 caracteres, una mayuscula y un simbolo (./-_@)");
            return;
        }

        const response = await fetch("https://localhost:52291/api/usuario/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                nombreUsuario,
                contrasena: encriptarMD5(contrasena)
            }),
        });

        if (response.ok) {
            localStorage.setItem("usuario", nombreUsuario);
            setEstaLogueado(true);
            navigate("/usuarios");
        } else {
            alert("Credenciales incorrectas");
        }
    };

    const registrarse = async () => {
        if (!validarContrasena(contrasena)) {
            alert("La contrasena debe tener al menos 8 caracteres, una mayuscula y un simbolo (./-_@)");
            return;
        }

        const response = await fetch("https://localhost:52291/api/usuario/register", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                nombreUsuario,
                contrasena: encriptarMD5(contrasena)
            }),
        });

        if (response.ok) {
            alert("Registrado con exito");
        } else {
            alert("Error al registrar, Ya estas registrado o verifica tus credenciales");
        }
    };

    return (
        <div className="login-container">
            <h1>Login / Registro</h1>
            <input
                placeholder="Usuario"
                value={nombreUsuario}
                onChange={(e) => setNombreUsuario(e.target.value)}
            />
            <input
                type="password"
                placeholder="Contraseña"
                value={contrasena}
                onChange={(e) => setContrasena(e.target.value)}
            />
            <button onClick={iniciarSesion}>Iniciar sesión</button>
            <button onClick={registrarse}>Registrarse</button>
        </div>
    );
}

export default Login;
