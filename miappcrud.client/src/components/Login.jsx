import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import CryptoJS from "crypto-js";
import "./Login.css";

function Login({ setEstaLogueado }) {
    const [formData, setFormData] = useState({
        nombre: '',
        apellido: '',
        edad: '',
        genero: '',
        correo: '',
        provincia: '',
        ciudad: '',
        contrasena: '',
        confirmarContrasena: ''
    });
    const [provincias, setProvincias] = useState([]);
    const [ciudades, setCiudades] = useState([]);
    const [esRegistro, setEsRegistro] = useState(false);
    const [error, setError] = useState('');
    const [cargandoProvincias, setCargandoProvincias] = useState(false);
    const navigate = useNavigate();

    const encriptarMD5 = (str) => CryptoJS.MD5(str).toString().toUpperCase();

    useEffect(() => {
        const cargarProvincias = async () => {
            setCargandoProvincias(true);
            try {
                const response = await fetch("https://localhost:52291/api/usuario/provincias");
                if (!response.ok) throw new Error("Error al cargar provincias");
                const data = await response.json();
                setProvincias(data);
            } catch (err) {
                console.error("Error fetching provincias:", err);
                setError("Error al cargar provincias. Intente recargar la pagina.");
            } finally {
                setCargandoProvincias(false);
            }
        };
        cargarProvincias();
    }, []);

    const cargarCiudades = async (provincia) => {
        try {
            const response = await fetch(`https://localhost:52291/api/usuario/ciudades/${encodeURIComponent(provincia)}`);
            if (!response.ok) throw new Error("Error al cargar ciudades");
            const data = await response.json();
            setCiudades(data);
        } catch (err) {
            console.error("Error fetching ciudades:", err);
            setError("Error al cargar ciudades para la provincia seleccionada");
        }
    };

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));

        if (name === 'provincia') {
            cargarCiudades(value);
            setFormData(prev => ({ ...prev, ciudad: '' }));
        }
    };

    const validarContrasena = (pass) => {
        const tieneLongitud = pass.length >= 8;
        const tieneMayuscula = /[A-Z]/.test(pass);
        const tieneSimbolo = /[^\w\s]/.test(pass);
        return tieneLongitud && tieneMayuscula && tieneSimbolo;
    };

    const validarCorreo = (email) => /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);

    const validarFormulario = () => {
        if (esRegistro && !validarCorreo(formData.correo)) {
            setError("El correo electronico no es valido");
            return false;
        }

        if (!validarContrasena(formData.contrasena)) {
            setError("La contrasena debe tener al menos 8 caracteres, una mayuscula y un simbolo");
            return false;
        }

        if (esRegistro && formData.contrasena !== formData.confirmarContrasena) {
            setError("Las contrasenas no coinciden");
            return false;
        }

        if (esRegistro && (!formData.nombre || !formData.apellido || !formData.edad || !formData.genero || !formData.provincia || !formData.ciudad)) {
            setError("Todos los campos son obligatorios");
            return false;
        }

        setError('');
        return true;
    };

    const iniciarSesion = async () => {
        if (!validarFormulario()) return;

        try {
            const response = await fetch("https://localhost:52291/api/usuario/login", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    correo: formData.correo,
                    contrasena: encriptarMD5(formData.contrasena)
                }),
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.message || "Credenciales incorrectas");
            }

            localStorage.setItem("usuario", formData.correo);
            setEstaLogueado(true);
            navigate("/usuarios");
        } catch (err) {
            setError(err.message);
        }
    };

    const registrarse = async () => {
        if (!validarFormulario()) return;

        try {
            const usuario = {
                nombre: formData.nombre,
                apellido: formData.apellido,
                edad: parseInt(formData.edad),
                genero: formData.genero,
                correo: formData.correo,
                provincia: formData.provincia,
                ciudad: formData.ciudad,
                contrasena: formData.contrasena
            };

            const response = await fetch("https://localhost:52291/api/usuario/register", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(usuario),
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.message || "Error al registrar");
            }

            alert("¡Registro exitoso! Ahora puedes iniciar sesion.");
            setEsRegistro(false);
            setFormData(prev => ({ ...prev, contrasena: '', confirmarContrasena: '' }));
        } catch (err) {
            setError(err.message);
        }
    };

    return (
        <div className="login-page">
            <div className="login-container">
                <h1 className="login-title">{esRegistro ? "Registro de Usuario" : "Inicio de Sesion"}</h1>

                <div className="login-form-box">
                    {error && <div className="error-message">{error}</div>}

                    {esRegistro && (
                        <>
                            <div className="form-group">
                                <label>Nombre</label>
                                <input
                                    className="login-input"
                                    name="nombre"
                                    value={formData.nombre}
                                    onChange={handleChange}
                                    required
                                />
                            </div>

                            <div className="form-group">
                                <label>Apellido</label>
                                <input
                                    className="login-input"
                                    name="apellido"
                                    value={formData.apellido}
                                    onChange={handleChange}
                                    required
                                />
                            </div>

                            <div className="form-group">
                                <label>Edad</label>
                                <input
                                    className="login-input"
                                    name="edad"
                                    type="number"
                                    min="1"
                                    max="120"
                                    value={formData.edad}
                                    onChange={handleChange}
                                    required
                                />
                            </div>

                            <div className="form-group">
                                <label>Genero</label>
                                <select
                                    className="login-input"
                                    name="genero"
                                    value={formData.genero}
                                    onChange={handleChange}
                                    required
                                >
                                    <option value="">Seleccione genero</option>
                                    <option value="Masculino">Masculino</option>
                                    <option value="Femenino">Femenino</option>
                                    <option value="Otro">Otro</option>
                                </select>
                            </div>

                            <div className="form-group">
                                <label>Provincia</label>
                                <select
                                    className="login-input"
                                    name="provincia"
                                    value={formData.provincia}
                                    onChange={handleChange}
                                    required
                                    disabled={cargandoProvincias}
                                >
                                    <option value="">{cargandoProvincias ? "Cargando..." : "Seleccione provincia"}</option>
                                    {provincias.map(provincia => (
                                        <option key={provincia} value={provincia}>{provincia}</option>
                                    ))}
                                </select>
                            </div>

                            <div className="form-group">
                                <label>Ciudad</label>
                                <select
                                    className="login-input"
                                    name="ciudad"
                                    value={formData.ciudad}
                                    onChange={handleChange}
                                    disabled={!formData.provincia || ciudades.length === 0}
                                    required
                                >
                                    <option value="">{formData.provincia ? "Seleccione ciudad" : "Primero seleccione provincia"}</option>
                                    {ciudades.map(ciudad => (
                                        <option key={ciudad} value={ciudad}>{ciudad}</option>
                                    ))}
                                </select>
                            </div>
                        </>
                    )}

                    <div className="form-group">
                        <label>Correo electronico</label>
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
                        <label>Contrasena</label>
                        <input
                            className="login-input"
                            name="contrasena"
                            type="password"
                            value={formData.contrasena}
                            onChange={handleChange}
                            required
                        />
                    </div>

                    {esRegistro && (
                        <div className="form-group">
                            <label>Confirmar contrasena</label>
                            <input
                                className="login-input"
                                name="confirmarContrasena"
                                type="password"
                                value={formData.confirmarContrasena}
                                onChange={handleChange}
                                required
                            />
                        </div>
                    )}

                    <div className="button-group">
                        {!esRegistro ? (
                            <>
                                <button
                                    type="button"
                                    className="login-button primary"
                                    onClick={iniciarSesion}
                                >
                                    Iniciar sesion
                                </button>
                                <button
                                    type="button"
                                    className="login-button secondary"
                                    onClick={() => setEsRegistro(true)}
                                >
                                    Registrarse
                                </button>
                            </>
                        ) : (
                            <>
                                <button
                                    type="button"
                                    className="login-button primary"
                                    onClick={registrarse}
                                >
                                    Completar registro
                                </button>
                                <button
                                    type="button"
                                    className="login-button secondary"
                                    onClick={() => {
                                        setEsRegistro(false);
                                        setError('');
                                    }}
                                >
                                    Cancelar
                                </button>
                            </>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
}

export default Login;