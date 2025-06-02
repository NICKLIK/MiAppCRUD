import { useState } from "react";
import { useNavigate } from "react-router-dom";
import "./Login.css";

function LoginAdmin({ setEstaLogueado }) {
    const [formData, setFormData] = useState({
        correo: "",
        contrasena: "",
        claveAdmin: ""
    });
    const [error, setError] = useState("");
    const navigate = useNavigate();

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({
            ...prev,
            [name]: value
        }));
    };

    const iniciarSesionAdmin = async () => {
        if (!formData.correo || !formData.contrasena || !formData.claveAdmin) {
            setError("Debe ingresar todos los campos: correo, contraseña y clave admin");
            return;
        }

        try {
            const response = await fetch("https://localhost:52291/api/usuario/login-admin", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    correo: formData.correo.trim(),
                    contrasena: formData.contrasena.trim(),
                    claveAdmin: formData.claveAdmin.trim()
                })
            });

            if (!response.ok) {
                let errorMessage = "Credenciales incorrectas";
                try {
                    const errorData = await response.json();
                    errorMessage = errorData.message || errorMessage;
                } catch  {
                    // respuesta vacía o no JSON
                }
                throw new Error(errorMessage);
            }

            // Manejar respuesta segura (en caso de que esté vacía)
            const text = await response.text();
            const data = text ? JSON.parse(text) : {};

            localStorage.setItem("usuario", JSON.stringify(data));
            setEstaLogueado(true);

            if (data.rol === "ADMIN") {
                navigate("/gestion-admin");
            } else {
                navigate("/catalogo");
            }

        } catch (err) {
            setError(err.message);
        }
    };

    return (
        <div className="login-page">
            <div className="login-container">
                <h1 className="login-title">Inicio de Sesion Admin</h1>
                <div className="login-form-box">
                    {error && <div className="error-message">{error}</div>}

                    <div className="form-group">
                        <label>Correo electrónico</label>
                        <input
                            className="login-input"
                            name="correo"
                            type="email"
                            value={formData.correo}
                            onChange={handleChange}
                            required
                        />
                    </div>

                    <div className="form-group">
                        <label>Contraseña</label>
                        <input
                            className="login-input"
                            name="contrasena"
                            type="password"
                            value={formData.contrasena}
                            onChange={handleChange}
                            required
                        />
                    </div>

                    <div className="form-group">
                        <label>Clave de Administrador</label>
                        <input
                            className="login-input"
                            name="claveAdmin"
                            type="text"
                            value={formData.claveAdmin}
                            onChange={handleChange}
                            required
                        />
                    </div>

                    <div className="login-button-group">
                        <button
                            type="button"
                            className="login-button iniciar"
                            onClick={iniciarSesionAdmin}
                        >
                            Iniciar como Admin
                        </button>
                        <button
                            type="button"
                            className="login-button registrar"
                            onClick={() => navigate("/")}
                        >
                            Volver
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default LoginAdmin;
