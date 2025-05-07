<script lang="ts">
	let { sessaoAtual, trilhas } = $props();
</script>

<div class="absolute bottom-0 flex w-screen justify-end gap-5 border-t border-t-gray-300 bg-white">
	<div class="flex gap-5 p-1">
		{#each trilhas as trilha}
			<a
				href="/calendarios?trilha={trilha?.calendarioId}"
				rel="external"
				class="pr-3 hover:cursor-pointer hover:font-bold {trilha.trilha === sessaoAtual &&
					'font-bold text-blue-600'}"
			>
				{trilha.trilha}
			</a>
		{/each}
	</div>
</div>
