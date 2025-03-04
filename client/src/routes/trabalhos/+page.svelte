<script lang="ts">
	import type { PageProps } from './$types';

	let { data }: PageProps = $props();

	const trabalhos = data?.trabalhos;
</script>

<div class="flex flex-col items-center pt-12 min-h-[100%] prevent-select">
	<h1>Trabalhos</h1>

	<div class="flex gap-12 flex-wrap w-screen justify-between px-12">
		{#each trabalhos as trabalho}
			<a href="/trabalhos/{trabalho.id}" class="border rounded-sm w-1/6 h-64 p-4 flex flex-col items-center
			{!trabalho.emAndamento && 'bg-gray-100 opacity-50'}
			{trabalho.emAndamento && 'hover:border-2 hover:border-blue-600 hover:cursor-pointer'}
			">
				<p><strong>{trabalho.nome}</strong></p>
				<p>Grupos: {trabalho.totalGrupos}</p>
			</a>
		{/each}
	</div>
</div>