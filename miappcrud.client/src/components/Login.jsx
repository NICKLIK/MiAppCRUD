import { useState } from "react";
import { useNavigate } from "react-router-dom";
import CryptoJS from "crypto-js";
import "./Login.css";

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
            alert("La contraseña debe tener al menos 8 caracteres, una mayúscula y un símbolo (./-_@)");
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
            alert("La contraseña debe tener al menos 8 caracteres, una mayúscula y un símbolo (./-_@)");
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
            alert("Registrado con éxito");
        } else {
            alert("Error al registrar. Ya estás registrado o verifica tus credenciales");
        }
    };

    return (
        <div className="login-container">
            <h1 className="login-title">Bienvenid@. Ingresa con tu Cuenta</h1>
            <div className="login-form-box">
                <input
                    className="login-input"
                    placeholder="Usuario"
                    value={nombreUsuario}
                    onChange={(e) => setNombreUsuario(e.target.value)}
                />
                <input
                    className="login-input"
                    type="password"
                    placeholder="Contrasena"
                    value={contrasena}
                    onChange={(e) => setContrasena(e.target.value)}
                />
                <div className="login-button-group">
                    <button className="login-button iniciar" onClick={iniciarSesion}>
                        Iniciar sesion
                    </button>
                    <button className="login-button registrar" onClick={registrarse}>
                        Registrarse
                    </button>
                </div>
            </div>
        </div>
    );
}

export default Login;
