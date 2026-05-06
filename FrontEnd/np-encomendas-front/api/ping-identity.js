export default async function handler(request, response) {
  try {
    const res = await fetch('https://nporder-identiity-server.onrender.com');
    const status = res.status;
    return response.status(200).json({ message: 'Ping Identity enviado com sucesso', renderStatus: status });
  } catch (error) {
    return response.status(500).json({ error: 'Erro ao pingar Identity no Render' });
  }
}
