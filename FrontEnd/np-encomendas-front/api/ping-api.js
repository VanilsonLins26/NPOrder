export default async function handler(request, response) {
  try {
    const res = await fetch('https://backend-api-tk7o.onrender.com/api/ping');
    const status = res.status;
    return response.status(200).json({ message: 'Ping API Principal enviado com sucesso', renderStatus: status });
  } catch (error) {
    return response.status(500).json({ error: 'Erro ao pingar API Principal no Render' });
  }
}
