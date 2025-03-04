<script lang="ts">
	let { sessaoAtual, trilhas } = $props();

</script>

<div class="w-screen border-t absolute bottom-0 flex gap-5">
	<div class="flex gap-5 p-1">
		{#each trilhas as trilha}
			<p
				class="hover:cursor-pointer hover:font-bold pr-3 {trilha.trilha === sessaoAtual && 'font-bold text-blue-600'}">{trilha.trilha}</p>
		{/each}
	</div>
</div>
