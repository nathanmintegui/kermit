import type { PageLoad } from '../../../../.svelte-kit/types/src/routes';
import { mockRequest } from '$lib/utils/http-client.local';

export const load: PageLoad = async ({ fetch, params }) => {
	const response = await mockRequest(
		{
			data: [
				{ id: 1, nome: 'Trabalho 1', emAndamento: false, totalGrupos: 6 },
				{ id: 2, nome: 'Trabalho 2', emAndamento: false, totalGrupos: 11 },
				{ id: 3, nome: 'Trabalho 3', emAndamento: false, totalGrupos: 9 },
				{ id: 4, nome: 'Trabalho 4', emAndamento: true, totalGrupos: 5 }
			]
		},
		{ status: 200 }
	);

	const trabalhos = await response.json();

	return {
		trabalhos: trabalhos.data
	};
};
