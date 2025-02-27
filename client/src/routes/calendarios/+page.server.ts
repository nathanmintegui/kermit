import type { PageLoad } from './$types';

export const load: PageLoad = async ({ fetch, params }) => {
	const host = 'http://localhost:5201';

	const res = await fetch(`${host}/v1/calendarios`);
	const data = await res.json();

	return { data };
};
