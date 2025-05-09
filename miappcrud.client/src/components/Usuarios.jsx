import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import "./Usuarios.css";

function Usuarios() {
    const [usuarios, setUsuarios] = useState([]);
    const navigate = useNavigate();

    const cargarUsuarios = async () => {
        const response = await fetch("https://localhost:52291/api/usuario");
        const data = await response.json();
        setUsuarios(data);
    };

    useEffect(() => {
        cargarUsuarios();
    }, []);

    const cerrarSesion = () => {
        localStorage.removeItem("usuario");
        navigate("/");
    };

    const eliminar = async (id) => {
        await fetch(`https://localhost:52291/api/usuario/${id}`, { method: "DELETE" });
        cargarUsuarios();
    };

    return (
        <div className="usuarios-container">
            <h1>Información de los Usuarios Registrados</h1>
            <div className="usuarios-list">
                {usuarios.map((u) => (
                    <div key={u.id} className="usuario-item">
                        <div className="usuario-info">
                            <h3>{u.nombre} {u.apellido}</h3>
                            <p><strong>Correo:</strong> {u.correo}</p>
                            <p><strong>Edad:</strong> {u.edad}</p>
                            <p><strong>Género:</strong> {u.genero}</p>
                            <p><strong>Ubicación:</strong> {u.ciudad}, {u.provincia}</p>
                        </div>
                        <button
                            className="eliminar-button"
                            onClick={() => eliminar(u.id)}
                        >
                            Eliminar
                        </button>
                    </div>
                ))}
            </div>
            <button className="logout-button" onClick={cerrarSesion}>
                Cerrar sesión
            </button>
        </div>
    );
}

export default Usuarios;