import { mockRequest } from '$lib/utils/http-client.local';

export const load: PageLoad = async ({ fetch, params }) => {
	console.log(`GET /trabalhos/${params?.id}`);

	const response = await mockRequest(
		{
			data: {
				id: 1,
				nome: 'Trabalho 1',
				emAndamento: true,
				grupos: [
					{
						id: 1,
						nome: 'Grupo 1',
						integrantes: [
							{
								id: 1,
								nome: 'João da Silva',
								cargo: 'TL'
							},
							{
								id: 2,
								nome: 'Carlinhos Maia',
								cargo: 'DEV'
							},
							{
								id: 3,
								nome: 'Jhonny Bravo',
								cargo: 'DEV'
							}
						]
					},
					{
						id: 2,
						nome: 'Grupo 2',
						integrantes: [
							{
								id: 1,
								nome: 'João da Silva',
								cargo: 'TL'
							},
							{
								id: 2,
								nome: 'Carlinhos Maia',
								cargo: 'DEV'
							},
							{
								id: 3,
								nome: 'Jhonny Bravo',
								cargo: 'DEV'
							}
						]
					},
					{
						id: 3,
						nome: 'Grupo 3',
						integrantes: [
							{
								id: 1,
								nome: 'João da Silva',
								cargo: 'TL'
							},
							{
								id: 2,
								nome: 'Carlinhos Maia',
								cargo: 'DEV'
							},
							{
								id: 3,
								nome: 'Jhonny Bravo',
								cargo: 'DEV'
							}
						]
					},
					{
						id: 4,
						nome: 'Grupo 4',
						integrantes: [
							{
								id: 1,
								nome: 'João da Silva',
								cargo: 'TL'
							},
							{
								id: 2,
								nome: 'Carlinhos Maia',
								cargo: 'DEV'
							},
							{
								id: 3,
								nome: 'Jhonny Bravo',
								cargo: 'DEV'
							}
						]
					},
					{
						id: 5,
						nome: 'Grupo 5',
						integrantes: [
							{
								id: 1,
								nome: 'João da Silva',
								cargo: 'TL'
							},
							{
								id: 2,
								nome: 'Carlinhos Maia',
								cargo: 'DEV'
							},
							{
								id: 3,
								nome: 'Jhonny Bravo',
								cargo: 'DEV'
							}
						]
					},
					{
						id: 6,
						nome: 'Grupo 6',
						integrantes: [
							{
								id: 1,
								nome: 'João da Silva',
								cargo: 'TL'
							},
							{
								id: 2,
								nome: 'Carlinhos Maia',
								cargo: 'DEV'
							},
							{
								id: 3,
								nome: 'Jhonny Bravo',
								cargo: 'DEV'
							}
						]
					}
				]
			}
		},
		{ status: 200 }
	);

	const grupos = await response.json();

	return {
		grupos: grupos.data
	};
};
