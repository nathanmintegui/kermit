import type { PageLoad } from '../../../../.svelte-kit/types/src/routes';
import { mockRequest } from '$lib/utils/http-client.local';

export const load: PageLoad = async ({ fetch, params }) => {
	console.log("ID da url ", params?.id);

	const host = 'http://localhost:5201';

	const res = await fetch(`${host}/v1/calendarios`);
	const data = await res.json();

	const response = await mockRequest(
		{
			data: [
				{ id: 1, trilha: 'Geral' },
				{ id: 2, trilha: 'FullStack' },
				{ id: 3, trilha: 'Dados' },
				{ id: 4, trilha: 'QA' }
			]
		},
		{ status: 200 }
	);

	const trilhas = await response.json();

	return {
		calendario: data,
		trilhas: trilhas.data
	};
};
