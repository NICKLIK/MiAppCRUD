import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import "./Login.css";

function PerfilUsuario() {
    const navigate = useNavigate();
    const correoUsuario = localStorage.getItem("usuario");

    const [usuario, setUsuario] = useState(null);
    const [editando, setEditando] = useState(false);
    const [provincias, setProvincias] = useState([]);
    const [ciudades, setCiudades] = useState([]);
    const [mensaje, setMensaje] = useState("");

    useEffect(() => {
        if (!correoUsuario) {
            navigate("/login");
            return;
        }
        cargarUsuario();
        cargarProvincias();
    }, []);

    useEffect(() => {
        if (usuario?.provincia) {
            cargarCiudades(usuario.provincia);
        }
    }, [usuario?.provincia]);

    const cargarUsuario = async () => {
        const res = await fetch("https://localhost:52291/api/usuario");
        const data = await res.json();
        const usuarioEncontrado = data.find(u => u.correo === correoUsuario);
        if (!usuarioEncontrado) {
            alert("No se encontró el usuario.");
            navigate("/login");
        } else {
            setUsuario(usuarioEncontrado);
            localStorage.setItem(`perfilUsuario_${correoUsuario}`, JSON.stringify({
                nombre: usuarioEncontrado.nombre,
                apellido: usuarioEncontrado.apellido,
                cedula: usuarioEncontrado.cedula || "",
                provincia: usuarioEncontrado.provincia,
                ciudad: usuarioEncontrado.ciudad
            }));
        }
    };

    const cargarProvincias = async () => {
        const res = await fetch("https://localhost:52291/api/usuario/provincias");
        const data = await res.json();
        setProvincias(data);
    };

    const cargarCiudades = async (provincia) => {
        const res = await fetch(`https://localhost:52291/api/usuario/ciudades/${encodeURIComponent(provincia)}`);
        const data = await res.json();
        setCiudades(data);
    };

    const manejarCambio = (e) => {
        const { name, value } = e.target;
        setUsuario(prev => ({
            ...prev,
            [name]: value,
            ...(name === "provincia" && { ciudad: "" })
        }));
    };

    const manejarGuardar = async () => {
        setMensaje("");

        if (!usuario.nombre || !usuario.apellido || !usuario.edad || !usuario.genero ||
            !usuario.provincia || !usuario.ciudad || !usuario.contrasena) {
            setMensaje("Todos los campos requeridos deben estar completos.");
            return;
        }

        try {
            const res = await fetch(`https://localhost:52291/api/usuario/${usuario.id}`, {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(usuario)
            });

            if (!res.ok) {
                const data = await res.json();
                throw new Error(data.message || "Error al actualizar.");
            }

            localStorage.setItem(`perfilUsuario_${correoUsuario}`, JSON.stringify({
                nombre: usuario.nombre,
                apellido: usuario.apellido,
                cedula: usuario.cedula || "",
                provincia: usuario.provincia,
                ciudad: usuario.ciudad
            }));

            alert("Datos actualizados correctamente.");
            setEditando(false);
        } catch (err) {
            setMensaje(err.message);
        }
    };

    if (!usuario) return <div>Cargando perfil...</div>;

    return (
        <div className="gestion-admin-container">
            <h1>Perfil de Usuario</h1>

            <div style={{ maxWidth: "500px", margin: "0 auto", textAlign: "left" }}>
                {mensaje && <p style={{ color: "red" }}>{mensaje}</p>}

                <p><strong>Correo:</strong> {usuario.correo}</p>

                <label>Nombre:
                    <input type="text" name="nombre" value={usuario.nombre} onChange={manejarCambio} disabled={!editando} />
                </label>

                <label>Apellido:
                    <input type="text" name="apellido" value={usuario.apellido} onChange={manejarCambio} disabled={!editando} />
                </label>

                <label>Cédula:
                    <input type="text" name="cedula" value={usuario.cedula || ""} onChange={manejarCambio} disabled={!editando} />
                </label>

                <label>Edad:
                    <input type="number" name="edad" value={usuario.edad} onChange={manejarCambio} disabled={!editando} />
                </label>

                <label>Género:
                    <select name="genero" value={usuario.genero} onChange={manejarCambio} disabled={!editando}>
                        <option value="">Seleccione</option>
                        <option value="Masculino">Masculino</option>
                        <option value="Femenino">Femenino</option>
                        <option value="Otro">Otro</option>
                    </select>
                </label>

                <label>Provincia:
                    <select name="provincia" value={usuario.provincia} onChange={manejarCambio} disabled={!editando}>
                        <option value="">Seleccione provincia</option>
                        {provincias.map(p => (
                            <option key={p} value={p}>{p}</option>
                        ))}
                    </select>
                </label>

                <label>Ciudad:
                    <select name="ciudad" value={usuario.ciudad} onChange={manejarCambio} disabled={!editando || !usuario.provincia}>
                        <option value="">Seleccione ciudad</option>
                        {ciudades.map(c => (
                            <option key={c} value={c}>{c}</option>
                        ))}
                    </select>
                </label>

                <label>Contraseña:
                    <input type="password" name="contrasena" value={usuario.contrasena} onChange={manejarCambio} disabled={!editando} />
                </label>
            </div>

            <div className="gestion-admin-botones" style={{ marginTop: "2rem" }}>
                {!editando ? (
                    <button onClick={() => setEditando(true)}>Editar Información</button>
                ) : (
                    <>
                        <button onClick={manejarGuardar}>Guardar Cambios</button>
                        <button onClick={() => { setEditando(false); cargarUsuario(); }}>Cancelar</button>
                    </>
                )}
                <button onClick={() => navigate("/gestion-usuario")}>Regresar</button>
            </div>
        </div>
    );
}

export default PerfilUsuario;
