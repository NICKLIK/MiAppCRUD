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
            <h1>Informacion de los Usuarios Registrados</h1>
            <div className="usuarios-list">
                {usuarios.map((u) => (
                    <div key={u.id} className="usuario-item">
                        <div className="usuario-info">
                            <h3>{u.nombre} {u.apellido}</h3>
                            <p><strong>Correo:</strong> {u.correo}</p>
                            <p><strong>Edad:</strong> {u.edad}</p>
                            <p><strong>Genero:</strong> {u.genero}</p>
                            <p><strong>Ubicacion:</strong> {u.ciudad}, {u.provincia}</p>
                        </div>
                        <button
                            className="eliminar-button"
                            onClick={() => eliminar(u.id)}
                        >
                            Eliminar
                        </button>

                        <button
                            className="ver-button"
                            onClick={() => navigate(`/usuarios/${u.id}`)}
                        >
                            Ver
                        </button>

                    </div>
                ))}
            </div>
            <button className="logout-button" onClick={cerrarSesion}>
                Cerrar sesion
            </button>

        </div>
    );
}

export default Usuarios;