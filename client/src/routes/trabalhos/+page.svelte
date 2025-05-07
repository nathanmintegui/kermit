<script lang="ts">
	import type { PageProps } from './$types';
	import Footer from './components/Footer.svelte';

	let { data }: PageProps = $props();

	const trabalhos = data?.trabalhos;
</script>

<div id="page" class="prevent-select page-background flex min-h-[100%] flex-col items-center pt-12">
	<h1>Trabalhos</h1>

	<div class="flex w-screen flex-wrap justify-between gap-12 px-12">
		{#each trabalhos as trabalho}
			<a
				href="/trabalhos/{trabalho.id}"
				class="flex h-64 w-1/6 flex-col items-center rounded-sm border p-4
			{!trabalho.emAndamento && 'bg-gray-100'}
			{trabalho.emAndamento && 'hover:cursor-pointer hover:border-2 hover:border-blue-600'}
			"
			>
				<p><strong>{trabalho.nome}</strong></p>
				<p>Grupos: {trabalho.totalGrupos}</p>
			</a>
		{/each}
	</div>
	<Footer />
</div>

<style>
	#page {
		background-color: rgba(255, 255, 255, 1);
		background-image: radial-gradient(rgba(0, 0, 0, 0.2) 0.9px, rgba(255, 255, 255, 1) 0.9px);
		background-size: 18px 18px;
	}
</style>
